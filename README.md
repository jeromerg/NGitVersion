NGitVersion
===========

Lightweight solution to support git-based Automatic versioning of C# and C++ (CLI and Native) DLLs in msbuild / visual studio environment.

Usage
-----
1. Copy the *NGitVersion* project into your visual studio project structure
2. Add version-support to your existing projects by following the instructions described in the file [NGitVersion/src/NGitVersion/Resource/Readme.md](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/Resource/Readme.md)
3. If needed, change the Major, Minor or Build Versions, by editing the constants in model file [NGitVersion/src/NGitVersion/Model/Model.cs](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/Model/Model.cs).

*NGitVersion* sets automatically the "revision number", the fourth placeholder of the version scheme.
The resulting two assembly version fields *File Version* and *Product Version* have the following patterns:
- *File Version*: (Example: `1.0.0.752`). It has the following pattern `MAJOR.MINOR.BUILD.GitIncrementalVersion`
Where 
    - `MAJOR`, `MINOR`, `BUILD` are constants defined in the *Model.cs* file
    - `GitIncrementalVersion` is the amount of commit from the root to the current commit
- *Product Version*: (Example: `0.0.0.12, Hash b716d1b, BuildConfig DEBUG, HasLocalChange True`). It contains additionally to the *File Version*:
    - The git "short hash" of the local checked-out commit 
    - The build configuration (DEBUG / RELEASE)
    - The information about whether some files have been locally edited
  

- *NGitVersion*'s implementation is simple a straight-forward: it is based on [StringTemplate](https://github.com/antlr/stringtemplate4) and [LibGit2Sharp](https://github.com/libgit2/libgit2sharp) and  You can easily customize it to suit your need.
