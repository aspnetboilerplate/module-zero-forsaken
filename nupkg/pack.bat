REM "..\tools\gitlink\GitLink.exe" ..\ -u https://github.com/aspnetboilerplate/module-zero -c release

@ECHO OFF
SET /P VERSION_SUFFIX=Please enter version-suffix (can be left empty): 

dotnet "pack" "..\src\Abp.Zero" -c "Release" -o "." --version-suffix "%VERSION_SUFFIX%"
dotnet "pack" "..\src\Abp.Zero.Owin" -c "Release" -o "." --version-suffix "%VERSION_SUFFIX%"
dotnet "pack" "..\src\Abp.Zero.AspNetCore" -c "Release" -o "." --version-suffix "%VERSION_SUFFIX%"
dotnet "pack" "..\src\Abp.Zero.Ldap" -c "Release" -o "." --version-suffix "%VERSION_SUFFIX%"
dotnet "pack" "..\src\Abp.Zero.EntityFramework" -c "Release" -o "." --version-suffix "%VERSION_SUFFIX%"
dotnet "pack" "..\src\Abp.Zero.EntityFrameworkCore" -c "Release" -o "." --version-suffix "%VERSION_SUFFIX%"
dotnet "pack" "..\src\Abp.Zero.NHibernate" -c "Release" -o "." --version-suffix "%VERSION_SUFFIX%"

pause
