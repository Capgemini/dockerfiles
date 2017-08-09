#load "helpers.cake"

var target = Argument("target", "Default");

var buildFile =
    HasArgument("buildFile") ? Argument<string>("buildFile") : 
    AppVeyor.IsRunningOnAppVeyor ? "build.ps1" :
    TravisCI.IsRunningOnTravisCI ? "build.sh" : "build.ps1";

var testFile =
    HasArgument("testFile") ? Argument<string>("testFile") :
    AppVeyor.IsRunningOnAppVeyor ? "test.ps1" :
    TravisCI.IsRunningOnTravisCI ? "test.sh" : "test.ps1";

var deployFile =
    HasArgument("deployFile") ? Argument<string>("deployFile") :
    AppVeyor.IsRunningOnAppVeyor ? "deploy.ps1" :
    TravisCI.IsRunningOnTravisCI ? "deploy.sh" : "deploy.ps1";

Task("Build")
    .Does(() =>
    {
        Debug("Looking for build file with an identifier of -> " + buildFile);

        var files = GetLastCommitChanges(buildFile);

        foreach(var f in files)
        {
            ExecuteScript(f);
        }
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        Debug("Looking for test file with an identifier of -> " + testFile);

        var files = GetLastCommitChanges(testFile);

        foreach(var f in files)
        {
            ExecuteScript(f);
        }
    });

Task("Deploy")
    .IsDependentOn("Test")
    .Does(() =>
    {
        Debug("Looking for depoy file with an identifier of -> " + deployFile);

        var files = GetLastCommitChanges(deployFile);

        foreach(var f in files)
        {
            ExecuteScript(f);
        }
    });

Task("Default")
    .IsDependentOn("Deploy");

RunTarget(target);