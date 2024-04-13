# LD55

TODO: make game

## Merging Unity Files

Use `UnityYAMLMerge`, a CLI tool.

Unity's tutorials on this: https://docs.unity3d.com/2019.3/Documentation/Manual/SmartMerge.html and https://learn.unity.com/tutorial/working-with-yamlmerge#

Find the path to it. On Mac, it was
`/Applications/Unity/Hub/Editor/2022.3.24f1/Unity.app/Contents/Tools/UnityYAMLMerge`.
On PC 
`C:\\Program Files\\Unity\\Hub\\Editor\\2022.3.24f1\\Editor\\Data\\Tools\\UnityYAMLMerge`.
You can use `fd` to find the file.

In your `~/.gitconfig`, add:
```
[merge]
tool = unityyamlmerge

[mergetool "unityyamlmerge"]
trustExitCode = false
cmd = '<path to UnityYAMLMerge>' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
```

Then when you do `git pull`, you'll still get merge conflicts.
But then you can run `git mergetool` and that will run `UnityYAMLMerge`, hopefully resolving the conflicts in a smart way.
After that you can do `git commit` to finish the merge.