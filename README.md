# dfs-utils
Command-line tools for manipulating MIKE dfs files. 

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

## Examples
Examples can be found in the examples folder including some small test data files. 

### From the commandline
Runnable examples can be found in (CmdLineExamples.bat)[./examples/CmdLineExamples.bat]. 

To get a help text just run the following

    > DfsUtils

Most tools use this overall syntax

    > DfsUtils [toolname] [inputfile] [arguments] [outputfile] 

To add a constant to a file (and output results to a new file):

    > DfsUtils AddConstant infile1.dfsu 0.34 outfile.dfsu

NOTE: here the file is dfsu but it could be any type of dfs file. 



### From MATLAB
Runnable examples can be found in (MatlabExamplesDfsUtils.m)[./examples/MatlabExamplesDfsUtils.m]

### From Python