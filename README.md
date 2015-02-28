ASP.NET Boilerplate - Module Zero
===========
[![Build status](https://ci.appveyor.com/api/projects/status/n02ctnbvhjeduuov/branch/master?svg=true)](https://ci.appveyor.com/project/SecComm/module-zero/branch/master)


What is 'module zero'
----------

'Module zero' is the first module for ASP.NET Boilerplate that includes following features:

* Implements ASP.NET Identity framework for User and Role management.
* Provides a Role and Permission based authorization system.
* Provides infrastructure to develop multi-tenant applications.
* Implements Setting system of ASP.NET Boilerplate to store Tenant, Application and User level settings in the database.
* And much more...
 
__IMPORTANT NOTE__: This project is not production ready yet and not released. But you can try it early.

Sample Project
-------------------

There is a sample project in the **sample** folder. To run it:

- Open it in VS2013
- Check connection string in web.config and change if you want to.
- Run DB migrations (Run 'Update-Database' command from Package Manager Console while ModuleZeroSampleProject.EntityFramework is selected as default project) to create database and initial data.
- Run the application! You will see the login form:
 
![alt login form](https://raw.githubusercontent.com/aspnetboilerplate/module-zero/master/doc/login-form.png)

See running application on http://qasample.aspnetboilerplate.com/

User name: admin or emre

Password: 123qwe

After login, a question list is shown:

![alt login form](https://raw.githubusercontent.com/aspnetboilerplate/module-zero/master/doc/question-list.png)

This sample is still being developed. A CodeProject article will be written when it's finished.
