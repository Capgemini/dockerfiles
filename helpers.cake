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
        Debug("Following file has changed:" + gitDiffFile);

        if (gitDiffFile.Path.EndsWith(checkFile))
        {   
            cc.Add(gitDiffFile.Path);
        }
    }

    return cc;
}

public void ExecuteScript(string scripto)
{
    Information("Starting to execute file -> {0}", scripto);

    var resultCollection = StartPowershellFile(scripto);

    var returnCode = int.Parse(resultCollection[0].BaseObject.ToString());

    Information("Result -> {0}", returnCode);
}