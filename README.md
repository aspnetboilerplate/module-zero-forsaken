# ASP.NET Boilerplate - Module Zero

AppVeyor: [![Build status](https://ci.appveyor.com/api/projects/status/56e9cadwavg3lj64?svg=true)](https://ci.appveyor.com/project/hikalkan/module-zero)

## What is 'module zero'?

This is an <a href="https://aspnetboilerplate.com/" target="_blank">ASP.NET Boilerplate</a> module integrated to Microsoft <a href="https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/introduction-to-aspnet-identity" target="_blank">ASP.NET Identity</a>.

Implements abstract concepts of ASP.NET Boilerplate framework:

* <a href="https://aspnetboilerplate.com/Pages/Documents/Setting-Management" target="_blank">Setting store</a>
* <a href="https://aspnetboilerplate.com/Pages/Documents/Audit-Logging" target="_blank">Audit log store</a>
* <a href="https://aspnetboilerplate.com/Pages/Documents/Background-Jobs-And-Workers" target="_blank">Background job store</a>
* <a href="https://aspnetboilerplate.com/Pages/Documents/Feature-Management" target="_blank">Feature store</a>
* <a href="https://aspnetboilerplate.com/Pages/Documents/Notification-System" target="_blank">Notification store</a>
* <a href="https://aspnetboilerplate.com/Pages/Documents/Authorization" target="_blank">Permission checker</a>

Also adds common enterprise application features:

* **<a href="https://aspnetboilerplate.com/Pages/Documents/Zero/User-Management" target="_blank">User</a>, <a href="https://aspnetboilerplate.com/Pages/Documents/Zero/Role-Management" target="_blank">Role</a> and <a href="https://aspnetboilerplate.com/Pages/Documents/Zero/Permission-Management" target="_blank">Permission</a>** management for applications require authentication and authorization.
* **<a href="https://aspnetboilerplate.com/Pages/Documents/Zero/Tenant-Management" target="_blank">Tenant</a> and <a href="https://aspnetboilerplate.com/Pages/Documents/Zero/Edition-Management" target="_blank">Edition</a>** management for SaaS applications.
* **<a href="https://aspnetboilerplate.com/Pages/Documents/Zero/Organization-Units" target="_blank">Organization Units</a>** management.
* **<a href="https://aspnetboilerplate.com/Pages/Documents/Zero/Language-Management" target="_blank">Language and localization</a> text** management.
* **<a href="https://aspnetboilerplate.com/Pages/Documents/Zero/Identity-Server" target="_blank">Identity Server 4</a>** integration.

Module zero packages defines entities and implements base domain logic for these concepts.

## Startup Templates

You can create your project from startup templates to easily start with module zero:

* <a href="https://aspnetboilerplate.com/Pages/Documents/Zero/Startup-Template-Angular" target="_blank">ASP.NET Core & Angular</a> based startup project.
* <a href="https://aspnetboilerplate.com/Pages/Documents/Zero/Startup-Template-Core" target="_blank">ASP.NET Core MVC & jQuery</a> based startup project.
* <a href="https://aspnetboilerplate.com/Pages/Documents/Zero/Startup-Template" target="_blank">ASP.NET Core MVC 5.x / Angularjs</a> based startup project.
 
A screenshot from ASP.NET Core based startup template:

![](doc/img/module-zero-core-template.png)

## Documentation

http://www.aspnetboilerplate.com/Pages/Documents

## Nuget Packages

### ASP.NET Core Identity Packages

Packages integrated to <a href="https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/introduction-to-aspnet-identity" target="_blank">ASP.NET Core Identity</a> and <a href="http://identityserver.io/" target="_blank">Identity Server 4</a> (supports .net standard).

|Package|Status|
|:------|:-----:|
|Abp.ZeroCore|[![NuGet version](https://badge.fury.io/nu/Abp.ZeroCore.svg)](https://badge.fury.io/nu/Abp.ZeroCore)|
|Abp.ZeroCore.EntityFrameworkCore|[![NuGet version](https://badge.fury.io/nu/Abp.ZeroCore.EntityFrameworkCore.svg)](https://badge.fury.io/nu/Abp.ZeroCore.EntityFrameworkCore)|
|Abp.ZeroCore.IdentityServer4|[![NuGet version](https://badge.fury.io/nu/Abp.ZeroCore.IdentityServer4.svg)](https://badge.fury.io/nu/Abp.ZeroCore.IdentityServer4)|
|Abp.ZeroCore.IdentityServer4.EntityFrameworkCore|[![NuGet version](https://badge.fury.io/nu/Abp.ZeroCore.IdentityServer4.EntityFrameworkCore.svg)](https://badge.fury.io/nu/Abp.ZeroCore.IdentityServer4.EntityFrameworkCore)|

### ASP.NET Identity Packages

Packages integrated to <a href="https://www.asp.net/identity" target="_blank">ASP.NET Identity</a> 2.x.

|Package|Status|
|:------|:-----:|
|Abp.Zero|[![NuGet version](https://badge.fury.io/nu/Abp.Zero.svg)](https://badge.fury.io/nu/Abp.Zero)|
|Abp.Zero.Owin|[![NuGet version](https://badge.fury.io/nu/Abp.Zero.Owin.svg)](https://badge.fury.io/nu/Abp.Zero.Owin)|
|Abp.Zero.AspNetCore|[![NuGet version](https://badge.fury.io/nu/Abp.Zero.AspNetCore.svg)](https://badge.fury.io/nu/Abp.Zero.AspNetCore)|
|Abp.Zero.EntityFramework|[![NuGet version](https://badge.fury.io/nu/Abp.Zero.EntityFramework.svg)](https://badge.fury.io/nu/Abp.Zero.EntityFramework)|

### Shared Packages

Shared packages between Abp.ZeroCore.\* and Abp.Zero.\* packages.

|Package|Status|
|:------|:-----:|
|Abp.Zero.Common|[![NuGet version](https://badge.fury.io/nu/Abp.Zero.Common.svg)](https://badge.fury.io/nu/Abp.Zero.Common)|
|Abp.Zero.Ldap|[![NuGet version](https://badge.fury.io/nu/Abp.Zero.Ldap.svg)](https://badge.fury.io/nu/Abp.Zero.Ldap)|



