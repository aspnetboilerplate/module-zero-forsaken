var target = Argument("target", "Package");
var config = Argument("config", "Release");

Task("Clean")
   .Does(() =>
{
   // Clean directories.
   CleanDirectory("./output");
   //CleanDirectory("./output/bin");
   //CleanDirectories("./src/**/bin/" + config);
});

Task("Build")
   .IsDependentOn("Clean")
   .Does(() =>
{
   // Build the solution using MSBuild.
   MSBuild("./src/Abp.Zero.sln", settings => 
      settings.SetConfiguration(config));     
});

//Task("RunUnitTests")
//   .IsDependentOn("Build")
//   .Does(() =>
//{
//   // Run xUnit tests.
//   XUnit("./src/**/bin/" + config + "/*.Tests.dll");
//});

Task("CopyFiles")
   .IsDependentOn("Build")
   .Does(() =>
{
   var path = "./src/Abp.Zero";    
   var files = GetFiles(path + "/*");

   // Copy all exe and dll files to the output directory.
   CopyFiles(files, "./output/release");
});    

Task("Package")
   .IsDependentOn("Build")
   .Does(() =>
{
   // Zip all files in the bin directory.
   Zip("./src/Abp.Zero", "./output/ABPRelease.zip");
});

RunTarget(target);