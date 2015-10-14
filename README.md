ASP.NET Boilerplate - Module Zero
===========

What is 'module zero'
----------

Module-Zero is the module that implements abstract concepts of ASP.NET Boilerplate framework, also adds some useful stuff for enterprise web applications:

* Implements ASP.NET Identity framework for User and Role management.
* Provides a Role and Permission based authorization system.
* Provides infrastructure to develop multi-tenant applications.
* Implements Setting system of ASP.NET Boilerplate to store Tenant, Application and User level settings in the database.
* Implements audit logging.
* Implements session management.

How to start?
-------------------
Follow the documentation to start with module-zero: http://www.aspnetboilerplate.com/Pages/Documents/Zero/Installation

Setting system
----------
Every application need to store some settings and use these settings in somewhere in the application. ASP.NET Boilerplate provides a strong infrastructure to store/retrieve application, tenant and user level settings usable both in server and client sides.

A setting is a name-value string pair that is generally stored in a database (or another source). We can store non-string values by converting to string.

**Check the full documentation:** http://aspnetboilerplate.com/Pages/Documents/Setting-Management

Authorization
----------
Almost all enterprise applications use authorization in some level. Authorization is used to check if a user is allowed to perform some specific operation in the application. ASP.NET Boilerplate defines a permission based infrastructure to implement authorization.

**Check the full documentation:** http://aspnetboilerplate.com/Pages/Documents/Authorization




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
