#addin nuget:?package=Cake.Git

public void PrintLastCommitChanges()
{
    var lastCommit = GitLogTip(".");

    Information("Last commit sha " + lastCommit.Sha);

    var gitDiffFiles = GitDiff(".", lastCommit.Sha);

    foreach(var gitDiffFile in gitDiffFiles)
    {
        Debug("File change detected -> {0}", gitDiffFile);
    }
}

public List<string> GetLastCommitChanges(string checkFile)
{
    List<string> cc = new List<string>();

    var lastCommit = GitLogTip(".");

    var gitDiffFiles = GitDiff(".", lastCommit.Sha);

    foreach(var gitDiffFile in gitDiffFiles)
    {
        bool isDirectory = !string.IsNullOrWhiteSpace(System.IO.Path.GetDirectoryName(gitDiffFile.Path));

        if (gitDiffFile.Exists && gitDiffFile.Path.EndsWith(checkFile) && isDirectory)
        {   
            cc.Add(gitDiffFile.Path);
        }
    }

    return cc;
}

public void ExecuteScript(string scripto)
{
    var scriptRunner =
        AppVeyor.IsRunningOnAppVeyor ? "powershell" :
        TravisCI.IsRunningOnTravisCI ? "bash" : "powershell";

    Information("Starting to execute file -> {0}", scripto);

    using(var process = StartAndReturnProcess(scriptRunner, new ProcessSettings{ Arguments = scripto }))
    {
        process.WaitForExit();

        Information("Exit code -> {0}", process.GetExitCode());
    }
}