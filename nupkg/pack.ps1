# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$slnPath = Join-Path $packFolder "../"
$srcPath = Join-Path $slnPath "src"

# List of projects
$projects = (
    "Abp.Zero.Common",
    "Abp.Zero.Ldap",
    "Abp.Zero",
    "Abp.Zero.Owin",
    "Abp.Zero.AspNetCore",
    "Abp.Zero.EntityFramework",
    "Abp.Zero.EntityFrameworkCore",
    "Abp.Zero.NHibernate",
    "Abp.ZeroCore",
    "Abp.ZeroCore.EntityFrameworkCore"
)

# Rebuild solution
Set-Location $slnPath
& dotnet msbuild /t:Rebuild /p:Configuration=Release

# Copy all nuget packages to the pack folder
foreach($project in $projects) {
    
    $projectFolder = Join-Path $srcPath $project

    # Create nuget pack
    Set-Location $projectFolder
    & dotnet msbuild /t:pack /p:Configuration=Release /p:IncludeSymbols=true

    # Copy nuget package
    $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $project + ".*.nupkg")
    Move-Item $projectPackPath $packFolder

}

# Go back to the pack folder
Set-Location $packFolder