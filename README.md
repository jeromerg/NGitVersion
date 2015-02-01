NGitVersion
===========

Auto-increment your DLL versions based on git history.

Out-of-the-box *NGitVersion* populates your DLLs with the following versions:

![screenshot](/doc/screenshot/dll_version.PNG)

- File Version: `1.0.0.34`, where `34` is the *Git* auto-incremented version
- Product Version: `1.0.0.34, Hash b716d1b, BuildConfig DEBUG, HasLocalChange True`, where:
    - `Hash` is the git "short hash" of the current commit 
    - `BuildConfig` is the build configuration (`DEBUG` / `RELEASE`)
    - `HasLocalChange` tells whether some file has been locally edited

*NGitVersion* introduces a C# project called *NGitVersion* that generates on build two files `GLobalAssemblyInfo.cs` and `GLobalAssemblyInfo.cpp`. Just reference the *NGitVersion* project and `GlobalAssemblyInfo.*` files in your existing projects in order to get it work with your own DLLs.

*NGitVersion* is only a few lines of code. It is based on the template engine [StringTemplate](https://github.com/antlr/stringtemplate4) and the git-client [LibGit2Sharp](https://github.com/libgit2/libgit2sharp). 

You can easily:

- Edit the templates, that generate the `GLobalAssemblyInfo.*` files
- Edit the underlying model `Model.cs` to change the major and minor version, product name...
- Extend the underlying model
    - to get more information from your Git repository (i.e. current branch, last commit description)
    - to get additional information from your build environment, like OS version/architecture, tool version, date...
- Add more templates
    -  i.e. to generate a file containing the commit-history, the patch of the local changes 

Usage
-----
### 1) Get *NGitVersion*

Clone [NGitVersion](https://github.com/jeromerg/NGitVersion) repository and copy its [/src/NGitVersion/](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/) subfolder to your target solution folder.

Alternatively, if you are confident with git subtree:
```
git subtree add --prefix my/target/folder https://github.com/jeromerg/NGitVersion master --squash
```

(update command)
```
git subtree pull --prefix my/target/folder https://github.com/jeromerg/NGitVersion master --squash
```

### 2) Add *NGitVersion* project to your Visual Studio solution

Add the `/src/NGitVersion/NGitVersion.csproj` to your solution.

### 3) Build

Typical issue: `Antlr4.StringTemplate` and `LibGit2Sharp` are not found: Fix the path to *NuGet* `packages` folder in the `NGitVersion.csproj` file:
		
- Either search and fix `..\packages\` string within in the project file
- Or remove/re-install nuget-dependencies `Antlr4.StringTemplate` and `LibGit2Sharp`

The build generates files into the `src\NGitVersion\Generated\` folder. Check that they exist and are properly generated from the templates located in `src\NGitVersion\Templates\`.

### Upgrade your projects

Now you can upgrade your projects to support *Git* automatic DLL versioning.

#### C# Project

1. Add *NGitVersion* project as project reference
2. Add manually the `ReferenceOutputAssembly` element to your `CSharp.csproj` (optional):

        <ProjectReference Include="..\..\NGitVersion\NGitVersion.csproj">
            <ReferenceOutputAssembly>false</ReferenceOutputAssembly>
            <Project>{badb59fb-1a14-42af-b314-152c4787f437}</Project>
            <Name>NGitVersion</Name>
         </ProjectReference>

From [MSDN](https://msdn.microsoft.com/en-us/library/47w1hdab.aspx): *If true, the assembly is used on the compiler command line during the build.* ... We just reference the *NGitVersion* project to ensure the correct build-order, so we disable this functionality.

3. Add file `src\NGitVersion\Generated\GlobalAssemblyInfo.cs` as a link (Add by using the *Add as link* button in *Add Existing Item* dialog)
4. Remove conflicting entries from the existing project `AssemblyInfo.cs`.
5. Build: Check that the built DLL contains the metadata defined in the generated `GlobalAssemblyInfo.cs` file.

#### Native and CLI C++ Project
1. Add *NGitVersion* project as project reference
   - Uncheck "Properties>Reference Assembly Output" in *Add Reference* dialog
2. Copy and include following files into your project (eventually merge with existing):
   - `NGitVersion\Resource\version.rc2` as *Resource Compiler*
   - `NGitVersion\Resource\Assembly.cpp` (embed .net metadata into C++ CLI Dlls, not relevant for native C++)
    
3. Fix all TODOs in both files, in particular the relative path to the generated `src\NGitVersion\Generated\GlobalAssemblyInfo.h` file.

4. Build: Check that the built DLL contains the metadata defined in the generated `GlobalAssemblyInfo.h` file. 

### Customize

- Customize model file [/src/NGitVersion/Model/Model.cs](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/Model/Model.cs) (major, minor, build versions, company name...)
- Edit/Add templates in [/src/NGitVersion/Templates/](https://github.com/jeromerg/NGitVersion/blob/master/src/NGitVersion/Templates/)
