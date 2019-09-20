# dfs-utils
Command-line tools for manipulating [MIKE dfs files](http://docs.mikepoweredbydhi.com/core_libraries/dfs/dfs-file-formats/). 

DfsUtils is a collection of tools which inputs 1 or 2 dfs files does some 
manipulation and the outputs a new dfs file of the same type. 

DfsUtils are accessible either through a .NET dll e.g. from MATLAB or Python 
or directly from the commandline using the exe. 


## Pre-requisites 
DfsUtils requires [MIKE SDK](https://www.mikepoweredbydhi.com/download/mike-2019/mike-sdk). 
Specifically, it uses the assemblies DHI.Generic.MikeZero.DFS and DHI.Generic.MikeZero.EUM. 


## Build
Run either the [build script](./build_DfsUtils.bat) or build from the Visual Studio. 
If your DHI.Generic.MikeZero assemblies is in a non-standard location then please update 
these references.


## Installation
If you have MIKE Zero 2019 installed, then simply copy the [binaries](./distribution/DfsUtils_MIKE2019.zip) to your MIKE bin-x64 folder 
(typically C:\Program Files (x86)\DHI\2019\bin\x64). 


## Tools

The following tools are currently available.

* AddConstant: add a constant to all data in the file
* Scale: multiply all data in file with a scale factor
* Sum: add to files together
* Diff: subtract to files from each other
* ExtractTimeSteps: extract specific timesteps between start and end with a stride


## Examples
Examples can be found in the examples folder including small test dfs files. 

### From the commandline
Runnable examples can be found in [CmdLineExamples.bat](./examples/CmdLineExamples.bat). 

To get a help text just run the following

    > DfsUtils

Most tools use this overall syntax

    > DfsUtils [toolname] [inputfile] [arguments] [outputfile] 

To add a constant to a file (and output results to a new file):

    > DfsUtils AddConstant infile1.dfsu 0.34 outfile.dfsu

NOTE: here the file is dfsu but it could be any type of dfs file. 



### From MATLAB
Runnable examples can be found in [MatlabExamplesDfsUtils.m](./examples/MatlabExamplesDfsUtils.m)

Use NET.addAssembly and the import statement to get access to the tools in DfsUtils.

	NET.addAssembly(path_to_assembly);
	import DHI.DFS.Utilities.*;
	
	differ = DfsDiff();
	differ.Run('file1.dfs1','file2.dfs1','difffile.dfs1');



### From Python
