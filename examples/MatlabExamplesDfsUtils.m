%% load assembly
NET.addAssembly([cd,'\..\DfsUtilsLib\bin\Release\DfsUtilsLib.dll']);
import DHI.DFS.Utilities.*;  % DfsAdd, DfsDiff, DfsScale, ...

%% example 1
multi = DfsScale();
filein  = 'data\TS1.dfs0';
fileout = 'output\scale2.dfs0';
multi.Run(filein, 2.0, fileout);

%% example 2
adder = DfsAddConstant();
filein  = 'data\TS1.dfs0';
fileout = 'output\add_constant.dfs0';
adder.Run(filein, 2.0, fileout);

%% example 3
summer = DfsAdd();
file1  = 'data\TS1.dfs0';
file2  = 'data\TS2.dfs0';
fileout = 'output\add.dfs0';
summer.Run(file1, file2, fileout);

%% example 3b
summer = DfsAdd();
file1  = 'data\TS1.dfs0';
file2  = 'data\TS2.dfs0';
fileout = 'output\add_90_10.dfs0';
summer.Run(file1, file2, .9, .1, fileout);

%% example 4
differ = DfsDiff();
file1  = 'data\wl_bc1.dfs1';
file2  = 'data\wl_bc2.dfs1';
fileout = 'output\diff.dfs1';
differ.Run(file1, file2, fileout);

%% example 5: extract time steps 
file_in = 'data\TS1.dfs0';
file_out = 'output\TS1_ks.dfs0';
dfsks = DfsKitchenSink(file_in, file_out);
dfsks.ExtractTimeSteps(0, 3);

%% example 5b (exact same as example 5, but different syntax)
file_out = 'output\TS1_extract.dfs0';
DfsTimeStepsExtractor.Extract(file_in, file_out, 0,3);

%% example 6: make time averaged file
file_in = 'data\TS1.dfs0';
file_out = 'output\TS1_avg.dfs0';
flattener = DfsFlatten();
flattener.Run(file_in, file_out);
