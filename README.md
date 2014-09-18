ASP.NET Boilerplate - Module Zero
===========

What is 'module zero'
----------

'Module zero' is the first module for ASP.NET Boilerplate that includes following features:

* Implements ASP.NET Identity framework for User and Role management.
* Provides a Role and Permission based authorization system.
* Provides infrastructure to develop multi-tenant applications.
* Implements Setting system of ASP.NET Boilerplate to store Tenant, Application and User level settings in the database.
* And much more...
 
__IMPORTANT NOTE__: This project is not production ready yet and not released. But, if you want to try early, you can follow the instructions below:

Manual installation
-------------------

* __Create ABP based project__. You must have an existing ASP.NET Boilerplate based solution. If not, you can create one on http://www.aspnetboilerplate.com/Templates
EntityFramework support for this module is not ready yet, choice __NHibernate__.

* __Install nuget packages__. I assume that your application's name is MyAbpApplication and your projects in solution are:
MyAbpApplication.Core
MyAbpApplication.Application
MyAbpApplication.Infrastructure.NHibernate
MyAbpApplication.WebApi
MyAbpApplication.Web
Then install nuget package __Abp.Zero__ for Core and Web projects, __Abp.Zero.NHibernate__ for Infrastructure.NHibernate project.

* __Define module dependencies__ (See http://www.aspnetboilerplate.com/Pages/Documents/Module-System#DocModuleDepend for documents).
Add AbpZeroModule dependency to your core module (MyAbpApplicationCoreModule class in MyAbpApplication.Core assembly)
Add AbpZeroNHibernateModule dependency to your MyAbpApplicationDataModule class.

* __Create database tables__. Use FluentMigrator migrations. Download AbpZeroDbMigrations.zip on https://github.com/aspnetboilerplate/module-zero/tree/master/temp, extract zip file, open RunMigrations.bat file, change connection string and run the bat file.

* __Install Identity framework and owin nuget packages__. Add Microsoft.Owin.Host.SystemWeb and Microsoft.AspNet.Identity.Owin packages to your web project.

* __Create Owin Startup class__. It will be something like that:

<pre lang="cs">
[assembly: OwinStartup(typeof(Startup))]

namespace MyAbpApplication.Web
{
    public class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void Configuration(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();
        }
    }
}</pre>

* __Use Identity framework__ in your controller to log in.

<pre lang="cs">
private void Login()
{
    //Surely, this informations should be sent from clients
    const string userName = "admin";
    const string password = "123qwe";
    const bool rememberMe = true;

    //Find the user
    var user = _userManager.FindByName(userName);

    //Check password
    if (!_userManager.CheckPassword(user, password))
    {
        throw new UserFriendlyException("User name or password is invalid");
    }

    //Create identity
    var identity = _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie).Result;

    //Sign in
    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
}
</pre>

Sample running application
--------------------------

Instead of manual installation, for a running sample application:
 
* __Create database tables__. Download AbpZeroDbMigrations.zip on https://github.com/aspnetboilerplate/module-zero/tree/master/temp, extract zip file and run the __bat__ file.
* __Download__ MyAbpApplicationWithModuleZero.zip on https://github.com/aspnetboilerplate/module-zero/tree/master/temp, extract zip file, open and run the solution on Visual Studio.

Further
-------

* Read ASP.NET Identity Framework's documentation for more information on ASP.NET Identity Framework.
* Download investigate source codes.
