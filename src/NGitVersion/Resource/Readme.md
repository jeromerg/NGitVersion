How to support git-incremental versioning for your VS Project
=============================================================

Prerequisite
------------

- source code ist in local git repository
- `git.exe` is in yout environment `PATH`

C# Project
----------

Steps to add versioning to a C# project:

1. Add `NGitVersion` project to your `CSharp-project` references 

2. (optional) Edit your `CSharp.csproj` file manually and add following `ReferenceOutputAssembly` 
   element to `NGitVersion` ProjectReference. Result should look like this:

        <ProjectReference Include="..\..\NGitVersion\NGitVersion.csproj">
            <ReferenceOutputAssembly>false</ReferenceOutputAssembly>
            <Project>{badb59fb-1a14-42af-b314-152c4787f437}</Project>
            <Name>NGitVersion</Name>
         </ProjectReference>

   ... it ensures, that the `CSharp` assembly doesn't reference the `NGitVersion` assembly but 
   only used at compile time.

3. "Add as Link" the `NGitVersion\Generated\GlobalAssemblyInfo.cs` file into the `CSharp` project
  - Right-click on project in Solution Explorer
  - Menu "Add>Existing Item..."
  - Browse and select the file `NGitVersion\Generated\GlobalAssemblyInfo.cs`
  - Do not click directly on "ADD BUTTON", but select "Add as Link" from the "Add Button ComboBox"

4. Build
   - eventually, remove conflicting entries from the already existing `AssemblyInfo.cs` 

5. Remark: DO NOT ADD ANY `GlobalAssemblyInfo.cs` file into source control repository!

C++ CLI Project
---------------

Steps to add versioning to a C++ CLI project (managed and unmanaged metadata):

1. Add `NGitVersion` project to your `CppCli`-project references 
   - Open project properties (Alt-Enter on project node in Solution Explorer)
   - Menu Common Properties, then click Button "Add New Reference..."
   - Browse and select the `NGitVersion` project
   - Set related properties "Properties>Reference Assembly Output" to false

2. Add pre-build copy command
   - Open project properties (Alt-Enter on project node in Solution Explorer)
   - don't forget to select "All Configurations"
   - Menu "Configuration Properties>Build Events>Pre-Build Event", then add following command lines:

        xcopy /Y "$(ProjectDir)..\..\NGitVersion\Generated\GlobalNativeAssemblyInfo.h" "$(ProjectDir)resource\"
        xcopy /Y "$(ProjectDir)..\..\NGitVersion\Generated\GlobalAssemblyInfo.cpp" "$(ProjectDir)resource\"

Remark: the reason why we copy the files and don't use links, is that the path to `stdafx.h` doesn't work with links

3. Build, in order to copy the files to `CppCli-project\resource\`
4. Include both files `GlobalNativeAssemblyInfo.h` and `GlobalAssemblyInfo.cpp` into the `CppCli`-project
5. Re-build: this time the global assembly infos are taken into account.
   - eventually, remove conflicting entries from the already existing `AssemblyInfo.cpp` (see warning output)

6. Copy the file `NGitVersion\Resource\version.rc2` into the `CppCli-project\resource` folder
7. Add the file `version.rc2` into the project. Set file property "Item Type" to *Resource Compiler*
8. Edit the file to set project specific information like "File Description"
9. Build and that's it!

10. Remark: DO NOT ADD `GlobalNativeAssemblyInfo.h` and `GlobalAssemblyInfo.cpp` files into source control repository!

Win32 C++ Project
=================

Step to add versioning to a Win32 C++ project (native metadata):

1. Add `NGitVersion` project to your `CppNative`-project references 
   - Open project properties (Alt-Enter on project node in Solution Explorer)
   - Menu Common Properties, then click Button "Add New Reference..."
   - Browse and select the NGitVersion project
   - Set related properties "Properties>Reference Assembly Output" to false

2. Add pre-build copy command
   - Open project properties (Alt-Enter on project node in Solution Explorer)
   - don't forget to select "All Configurations"
   - Menu "Configuration Properties>Build Events>Pre-Build Event", then add following command line:

        xcopy /Y "$(ProjectDir)..\..\NGitVersion\Generated\GlobalNativeAssemblyInfo.h" "$(ProjectDir)resource\"

3. Build in order to copy the files to `CppNative\resource\`
4. Include file `GlobalNativeAssemblyInfo.h` into the `CppNative` project
5. Copy the file `NGitVersion\Resource\version.rc2` into the `CppNative\resource` folder
6. Include the file `version.rc2` into the project. Set file property "Item Type" to *Resource Compiler*
7. Edit the file to set project specific information like "File Description"
8. Build and that's it!

9. Remark: DO NOT ADD `GlobalNativeAssemblyInfo.h` file into source control repository!
