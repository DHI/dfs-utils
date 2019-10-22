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

* Scale: multiply all data in file with a scale factor
* AddConstant: add a constant to all data in the file
* Sum: add to files together
* Diff: subtract to files from each other
* ExtractTimeSteps: extract specific timesteps between start and end with a stride
* TimeAverage: make a time averaged file 

## Examples
Examples can be found in the examples folder including small test dfs files. 

### From the commandline
Runnable examples can be found in [CmdLineExamples.bat](./examples/CmdLineExamples.bat). 

To get a help text just run the following

    > DfsUtils

Most tools use this overall syntax

    > DfsUtils [toolname] [inputfile] [arguments] [outputfile] 

Examples of all the available tools: 

    > DfsUtils Scale infile.dfsu 0.9 outfile.dfsu
    > DfsUtils AddConstant infile.dfsu 10.0 outfile.dfsu
    > DfsUtils Sum file1.dfs2 file2.dfs2 outfile.dfs2
    > DfsUtils Diff file1.dfs2 file2.dfs2 outfile.dfs2
    > DfsUtils ExtractTimeSteps infile.dfs0 outfile.dfs0 20 -1 2
    > DfsUtils TimeAverage infile.dfs1 outfile.dfs1

NOTE: in all cases the tool can be used with any type of dfs file. 



### From MATLAB
Runnable examples can be found in [MatlabExamplesDfsUtils.m](./examples/MatlabExamplesDfsUtils.m)

Use NET.addAssembly and the import statement to get access to the tools in DfsUtils.

	NET.addAssembly(path_to_dfsutillib);
	import DHI.DFS.Utilities.*;
	
	differ = DfsDiff();
	differ.Run('file1.dfs1','file2.dfs1','difffile.dfs1');



### From Python
Runnable examples can be found in [PythonExamples.py](./examples/PythonExamples.py)


	import clr
	clr.AddReference(path_to_dfsutillib)
	from DHI.DFS.Utilities import DfsTimeStepsExtractor
	DfsTimeStepsExtractor.Extract('.\data\TS1.dfs0', '.\output\pyt.dfs0', 0,3) 
