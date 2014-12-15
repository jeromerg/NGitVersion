NGitVersion
===========

Lightweight solution to support git-based Automatic versioning of C# and C++ (CLI and Native) DLLs in msbuild / visual studio environment.

Usage
-----
1. Copy the NGitVersion project into your visual studio project structure
2. Add version-support to your existing projects by following the instructions described in the file [NGitVersion/src/NGitVersion/Readme.md](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/Readme.md)
3. If needed, change the Major, Minor or Build Versions, by editing the constants in file [NGitVersion/src/NGitVersion/NGitVersion.cs](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/NGitVersion.cs).

NGitVersion sets automatically the "revision number", the fourth placeholder of the version scheme.
The resulting two assembly version fields *File Version* and *Product Version* have the following patterns:
- *File Version*: (Example: `1.0.0.752`). It has the following pattern `MAJOR.MINOR.BUILD.GitIncrementalVersion`
Where 
    - `MAJOR`, `MINOR`, `BUILD` are constants defined in the NGitVersion project
    - `GitIncrementalVersion` is the amount of commit from the root of all commits to the current commit
- *Product Version*: (Example: `0.8.8.273 - 08c9b52 DEBUG (with local changes)`). It contains additionally to the `File Version`:
    - The git "short hash" of the local checked-out commit 
    - The build target (DEBUG / RELEASE)
    - The information about whether some files have been locally edited
  

- `NGitVersion`'s implementation is simple a straight-forward. You can easily customize it to suit your need.
