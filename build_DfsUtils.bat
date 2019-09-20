set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
call %msBuildDir%\msbuild.exe .\DfsUtils.sln /p:Configuration=Release
mkdir dist
xcopy /y DfsUtilsRunner\bin\Release\DfsUtils.exe dist\