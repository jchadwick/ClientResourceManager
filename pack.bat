SET MSBUILD=%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
SET NUGET=.\tools\nuget.exe

REM %MSBUILD% /t:Clean,Rebuild /p:Configuration=Release

copy ClientResourceManager\bin\Release\ClientResourceManager.dll ClientResourceManager\NuGet\lib\
%NUGET% pack -BasePath ClientResourceManager\NuGet ClientResourceManager\NuGet\ClientResourceManager.nuspec

copy ClientResourceManager.Mvc\bin\Release\ClientResourceManager.Mvc.dll ClientResourceManager.Mvc\NuGet\lib\
%NUGET% pack -BasePath ClientResourceManager.Mvc\NuGet ClientResourceManager.Mvc\NuGet\ClientResourceManager.Mvc.nuspec
