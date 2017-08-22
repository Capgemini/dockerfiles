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
    AppVeyor.IsRunningOnAppVeyor ? "push.ps1" :
    TravisCI.IsRunningOnTravisCI ? "push.sh" : "push.ps1";

Task("BuildInfo")
    .Does(() =>
    {
        if (AppVeyor.IsRunningOnAppVeyor)
        {
            Information(
                @"Repository:
                Branch: {0}
                Name: {1}
                Provider: {2}
                Scm: {3}
                Folder: {4}
                Id: {5}
                Number: {6}
                Version: {7}
                IsTag: {8}
                Tag Name: {9}",
                AppVeyor.Environment.Repository.Branch,
                AppVeyor.Environment.Repository.Name,
                AppVeyor.Environment.Repository.Provider,
                AppVeyor.Environment.Repository.Scm,
                AppVeyor.Environment.Build.Folder,
                AppVeyor.Environment.Build.Id,
                AppVeyor.Environment.Build.Number,
                AppVeyor.Environment.Build.Version,
                AppVeyor.Environment.Repository.Tag.IsTag,
                AppVeyor.Environment.Repository.Tag.Name
                );
        }
        else if (TravisCI.IsRunningOnTravisCI)
        {
            Information(
                @"Repository:
                Branch: {0}
                BuildDirectory: {1}
                BuildId: {2}
                Tag: {3}
                TestResult: {4}
                Id: {5}
                Number: {6}
                Version: {7}
                Commit: {8}
                Tag Name: {9}",
                TravisCI.Environment.Build.Branch,
                TravisCI.Environment.Build.BuildDirectory,
                TravisCI.Environment.Build.BuildId,
                TravisCI.Environment.Build.Tag,
                TravisCI.Environment.Build.TestResult,
                TravisCI.Environment.Job.JobId,
                TravisCI.Environment.Job.JobNumber,
                TravisCI.Environment.Job.OSName,
                TravisCI.Environment.Repository.Commit,
                TravisCI.Environment.Repository.PullRequest
                );
        }

        PrintLastCommitChanges();
    });

Task("Build")
    .IsDependentOn("BuildInfo")
    .Does(() =>
    {
        Debug("Looking for build file with an identifier of -> " + buildFile);

        var dockerDirectories = GetLastCommitChanges(buildFile);

        foreach(var dd in dockerDirectories)
        {
            ExecuteScript(new string[]
            { 
                dd + "/" + buildFile,
                dd + "/" + testFile,
                dd + "/" + deployFile
            });
        }
    });

Task("Deploy")
    .IsDependentOn("Build")
    .Does(() =>
    {
        string [] subdirectoryEntries = System.IO.Directory.GetDirectories(".");

        foreach(string directory in subdirectoryEntries)
        {
            ExecuteScript(new string[]
            { 
                directory + "/" + deployFile
            });
        }
    });

Task("Default")
    .IsDependentOn("Deploy");

RunTarget(target);