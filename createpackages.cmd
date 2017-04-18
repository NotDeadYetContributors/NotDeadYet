set PackageVersion=
REM Restoring packages for each project in turn instead of the solution to avoid errors with the sample projects
dotnet restore ./src/NotDeadYet/NotDeadYet.csproj
dotnet restore ./src/NotDeadYet.MVC4/NotDeadYet.MVC4.csproj
dotnet restore ./src/NotDeadYet.Nancy/NotDeadYet.Nancy.csproj
dotnet restore ./src/NotDeadYet.WebApi/NotDeadYet.WebApi.csproj
dotnet pack ./src/NotDeadYet/NotDeadYet.csproj -c Release --include-source
dotnet pack ./src/NotDeadYet.MVC4/NotDeadYet.MVC4.csproj -c Release --include-source
dotnet pack ./src/NotDeadYet.Nancy/NotDeadYet.Nancy.csproj -c Release --include-source
dotnet pack ./src/NotDeadYet.WebApi/NotDeadYet.WebApi.csproj -c Release --include-source