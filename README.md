NGitVersion
===========

Lightweight solution to support git-based Automatic versioning of C# and C++ (CLI and Native) DLLs in visual studio.

Usage
-----
1. Copy the NGitVersion project into your visual studio project structure
2. Add version-support to your existing projects by following the instructions described in the file [NGitVersion/src/NGitVersion/Readme.md](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/Readme.md)
3. On need, change the Major, Minor or Build Versions, by editing the constants in file [NGitVersion/src/NGitVersion/NGitVersion.cs](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/NGitVersion.cs). The revision number will be set automatically to the git incremental version.

The resulting assembly versions have the following patterns:
- File Version: `MAJOR.MINOR.BUILD.GitIncrementalVersion`
Where `GitIncrementalVersion` is the amount of commit from the root of all commits to the current commit.
- Product Version: For example: `0.8.8.273 - 08c9b52 DEBUG (with local changes)`. It contains additionally 
    - The git "short hash" of the commit 
    - The build target (DEBUG / RELEASE)
    - The information about whether some files have been locally edited
  

- `NGitVersion`'s implementation is simple a straight-forward. You can easily customize it to suit your need.
