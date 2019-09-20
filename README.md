# dfs-utils
Command-line tools for manipulating MIKE dfs files

## Requirements

1. [MIKE SDK](https://www.mikepoweredbydhi.com/download/mike-2019/mike-sdk?ref=%7B5399F5D6-40C6-4BB2-8311-37B615A652C6%7D)
2. [.Net Framework 4.7.1](https://dotnet.microsoft.com/download/thank-you/net471-developer-pack)

## Build

Run `build_DfsUtils.bat` which will produce a `DfsUtils.exe` in the `dist` folder.

    C:\dfs-utils\dist\DfsUtils.exe
    Too few arguments
    Usage:
    >DfsUtils [toolname] [args]
    >DfsUtils Scale infile.dfsu 0.9 outfile.dfsu
    >DfsUtils Sum file1.dfs2 file2.dfs2 outfile.dfs2
    >DfsUtils Diff file1.dfs2 file2.dfs2 outfile.dfs2

