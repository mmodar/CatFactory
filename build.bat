cls
set initialPath=%cd%
set srcPath=%cd%\CatFactory\CatFactory
set testPath=%cd%\CatFactory\CatFactory.Tests
cd %srcPath%
dotnet build
cd %testPath%
dotnet test
cd %srcPath%
dotnet pack
cd %initialPath%
pause
