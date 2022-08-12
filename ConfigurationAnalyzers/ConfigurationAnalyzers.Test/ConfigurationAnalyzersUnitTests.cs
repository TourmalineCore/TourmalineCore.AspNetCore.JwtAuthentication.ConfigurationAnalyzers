using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = ConfigurationAnalyzers.Test.CSharpCodeFixVerifier<
    ConfigurationAnalyzers.ConfigurationAnalyzersAnalyzer,
    ConfigurationAnalyzers.ConfigurationAnalyzersCodeFixProvider>;

namespace ConfigurationAnalyzers.Test
{
    [TestClass]
    public class ConfigurationAnalyzersUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task ExampleTestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task ExampleTestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
        }
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";

            var expected = VerifyCS.Diagnostic("ConfigurationAnalyzers").WithLocation(0).WithArguments("TypeName");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }

        //This test only for the configuration analyzer debug. The test cannot pass successfully
        [TestMethod]
        public async Task CannotUseDefaultLoginMiddlewareAndCookieLoginMiddlewareAtTheSameTime_Net5()
        {
            const string test = @"
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TourmalineCore.AspNetCore.JwtAuthentication.Core;
using TourmalineCore.AspNetCore.JwtAuthentication.Core.Options;

namespace Net5Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseJwtAuthentication();
            app.UseDefaultLoginMiddleware();
            app.UseCookieLoginMiddleware(new CookieAuthOptions
            {
                Key = ""ExampleCookieName"",
            });
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //This test only for the configuration analyzer debug. The test cannot pass successfully
        [TestMethod]
        public async Task CannotUseDefaultLoginMiddlewareAndCookieLoginMiddlewareAtTheSameTime_Net6()
        {
            const string test = @"
using TourmalineCore.AspNetCore.JwtAuthentication.Core;
using TourmalineCore.AspNetCore.JwtAuthentication.Core.Options;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseCookieLoginMiddleware(new CookieAuthOptions
    {
        Key = ""ExampleCookieName"",
    });

app.UseJwtAuthentication();
//app.UseDefaultLoginMiddleware();

app.MapControllers();

app.Run();
";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
