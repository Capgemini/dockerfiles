#addin nuget:?package=Cake.Git
#addin nuget:?package=Cake.Powershell

public List<string> GetLastCommitChanges(string checkFile)
{
    List<string> cc = new List<string>();

    var lastCommit = GitLogTip(".");

    Debug("Last commit sha "+ lastCommit.Sha);

    var gitDiffFiles = GitDiff(".", lastCommit.Sha);

    foreach(var gitDiffFile in gitDiffFiles)
    {
        Debug("Following file has changed -> {0}", gitDiffFile);

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
    Information("Starting to execute file -> {0}", scripto);

    using(var process = StartAndReturnProcess(scripto))
    {
        process.WaitForExit();
        // This should output 0 as valid arguments supplied
        Information("Exit code -> {0}", process.GetExitCode());
    }    
}