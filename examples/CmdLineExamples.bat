set tool="..\DfsUtilsRunner\bin\Release\DfsUtils.exe"

REM example 1
REM tool ExtractSteps .\data\TS1.dfs0 .\output\TS1_ks.dfs0 0 3

REM example 4
tool Sum .\data\TS1.dfs0 .\data\TS1.dfs0 .\output\TS1_plus_TS2.dfs0



REM example 6
tool Scale .\data\TS1.dfs0 2.0 .\output\scale2.dfs0

REM example 7
REM tool AddConstant .\data\TS1.dfs0 2.0 .\output\scale2.dfs0
