@echo off

:Build
cls

if not exist tools\Cake\Cake.exe ( 
    echo Installing Cake...
    tools\NuGet.exe install Cake -OutputDirectory tools -ExcludeVersion -NonInteractive -Prerelease
)

echo Starting Cake...
tools\Cake\Cake.exe build.cake -target=Default -verbosity=diagnostic