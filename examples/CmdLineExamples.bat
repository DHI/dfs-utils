set tool="..\DfsUtilsRunner\bin\Release\DfsUtils.exe"

REM example 1
%tool% ExtractSteps .\data\TS1.dfs0 .\output\TS1_ks.dfs0 0 3

REM example 3
%tool% Diff .\data\TS1.dfs0 .\data\TS2.dfs0 .\output\TS1_minus_TS2.dfs0

REM example 4
%tool% Sum .\data\TS1.dfs0 .\data\TS2.dfs0 .\output\TS1_plus_TS2.dfs0


REM example 6
%tool% Scale .\data\TS1.dfs0 2.0 .\output\TS1_scale2.dfs0

REM example 7
%tool% AddConstant .\data\TS1.dfs0 2.0 .\output\TS1_plus2.dfs0
