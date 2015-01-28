How to support git-incremental versioning for your VS Project
=============================================================

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

Eventually, remove conflicting entries from your existing `AssemblyInfo.cs`.

4. Build and that's it!

C++ CLI Project
---------------

Steps to add versioning to a C++ CLI project (managed and unmanaged metadata):

1. Add `NGitVersion` project to your `CppCli`-project references 
   - Open project properties (Alt-Enter on project node in Solution Explorer)
   - Menu Common Properties, then click Button "Add New Reference..."
   - Browse and select the `NGitVersion` project
   - Set related properties "Properties>Reference Assembly Output" to false

2. Copy and include following files into your project (eventually merge with existing):
   - `NGitVersion\Resource\version.rc2` as *Resource Compiler*
   - `NGitVersion\Resource\Assembly.cpp` 
    
3. Fix all TODO in both files (a.o. the path to the generated `GlobalAssemblyInfo.h`)

4. Build: that's it!

Win32 C++ Project
=================

Same as C++ CLI except that you must omit to copy the Assembly.cpp.