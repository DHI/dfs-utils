import clr
import os
p = os.path.dirname(os.path.abspath(__file__))
# Get absolute path to DfsUtilsLib.dll, modify if needed
dllpath = os.path.join(p, r'..\DfsUtilsLib\bin\Release\DfsUtilsLib.dll')
clr.AddReference(dllpath)

from DHI.DFS.Utilities import DfsTimeStepsExtractor


# Example 1
DfsTimeStepsExtractor.Extract('.\data\TS1.dfs0', '.\output\pyt.dfs0', 0,3) 