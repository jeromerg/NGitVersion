NGitVersion
===========

Lightweight solution to support git-based Automatic versioning of C# and C++ (CLI and Native) DLLs in msbuild / visual studio environment.

*NGitVersion* sets automatically the "revision number", the last placeholder of the version scheme.
The resulting two assembly version fields *File Version* and *Product Version* have the following patterns:
- *File Version*: (Example: `1.0.0.752`). It has the following pattern `MAJOR.MINOR.BUILD.GitIncrementalVersion`
Where 
    - `MAJOR`, `MINOR`, `BUILD` are constants defined in the *Model.cs* file
    - `GitIncrementalVersion` is the amount of commit from the root to the current commit
- *Product Version*: (Example: `0.0.0.12, Hash b716d1b, BuildConfig DEBUG, HasLocalChange True`). It contains additionally:
    - The git "short hash" of the local checked-out commit 
    - The build configuration (DEBUG / RELEASE)
    - The information about whether some files have been locally edited
  

*NGitVersion*'s implementation is simple a straight-forward: it uses the template-engine [StringTemplate](https://github.com/antlr/stringtemplate4) and the git-client [LibGit2Sharp](https://github.com/libgit2/libgit2sharp). 

You can easily edit/add the provided templates or add new features.

Usage
-----
1. Copy the *NGitVersion* project into your visual studio project structure
2. Fix the path to packages folder with the `NGitVersion` project
    - Open `NGitVersion.csproj` with a text editor
    - search and replace `..\packages\` by the relative path to your path
    - Alternative: remove/re-install nuget-dependency `Antlr4.StringTemplate` and `LibGit2Sharp` 
3. Build the *NGitVersion* project within the solution, to check that it compiles well
4. For each of your project, add version-support by following the instructions: [NGitVersion/src/NGitVersion/Resource/Readme.md](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/Resource/Readme.md)
4. If needed customize:
    - Edit major, minor, build versions, company name and so on, by editing the model file: [NGitVersion/src/NGitVersion/Model/Model.cs](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/Model/Model.cs)
    - Change/Add templates `*.*.stg` in folder: [NGitVersion/src/NGitVersion/Templates/](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/Templates/)

Remark:

At step 1. if you are confident with git subtree, you can clone the *NGitVersion* repository as a subtree. In the root of your own git repository, call:

```
git subtree add --prefix src/Version https://github.com/jeromerg/NGitVersion master --squash
```

It creates a folder `MyGitRep/src/Version` and pull the `NGitVersion` git repository into it. You can later update/upgrade the code, with the following command:

```
git subtree pull --prefix src/Version https://github.com/jeromerg/NGitVersion master --squash
```
