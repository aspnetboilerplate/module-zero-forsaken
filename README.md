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

Sample Project
-------------------

There is a sample project in the **sample** folder. To run it:

- Open it in VS2013
- Check connection string in web.config and change if you want to.
- Run DB migrations (Run 'Update-Database' command from Package Manager Console while ModuleZeroSampleProject.EntityFramework is selected as default project) to create database and initial data.
- Run the application! You will see the login form:
 
![alt login form](https://raw.githubusercontent.com/aspnetboilerplate/module-zero/master/doc/login-form.png)

This sample is still being developed. A CodeProject article is will be written when it's finished.
