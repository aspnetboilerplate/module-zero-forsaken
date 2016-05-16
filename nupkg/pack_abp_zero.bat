"..\src\.nuget\NuGet.exe" "pack" "..\src\Abp.Zero\Abp.Zero.csproj" -Properties Configuration=Release -IncludeReferencedProjects -Symbols
"..\src\.nuget\NuGet.exe" "pack" "..\src\Abp.Zero.Ldap\Abp.Zero.Ldap.csproj" -Properties Configuration=Release -IncludeReferencedProjects -Symbols
"..\src\.nuget\NuGet.exe" "pack" "..\src\Abp.Zero.EntityFramework\Abp.Zero.EntityFramework.csproj" -Properties Configuration=Release -IncludeReferencedProjects -Symbols
"..\src\.nuget\NuGet.exe" "pack" "..\src\Abp.Zero.NHibernate\Abp.Zero.NHibernate.csproj" -Properties Configuration=Release -IncludeReferencedProjects -Symbols

ren *.symbols.nupkg *.temp_nupkg
del /a /f /q *.nupkg
ren *.temp_nupkg *.nupkg

GitLink.exe ..\ -u https://github.com/aspnetboilerplate/module-zero -c release

"..\src\.nuget\NuGet.exe" "pack" "..\src\Abp.Zero\Abp.Zero.csproj" -Properties Configuration=Release -IncludeReferencedProjects
"..\src\.nuget\NuGet.exe" "pack" "..\src\Abp.Zero.Ldap\Abp.Zero.Ldap.csproj" -Properties Configuration=Release -IncludeReferencedProjects
"..\src\.nuget\NuGet.exe" "pack" "..\src\Abp.Zero.EntityFramework\Abp.Zero.EntityFramework.csproj" -Properties Configuration=Release -IncludeReferencedProjects
"..\src\.nuget\NuGet.exe" "pack" "..\src\Abp.Zero.NHibernate\Abp.Zero.NHibernate.csproj" -Properties Configuration=Release -IncludeReferencedProjects

pause
