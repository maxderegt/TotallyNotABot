Set-Location -Path '.'
dotnet clean -c Release
dotnet restore
dotnet build -c Release -f netcoreapp2.0
dotnet publish -c Release -f netcoreapp2.0 -r ubuntu.16.10-x64

dotnet clean -c Release
dotnet restore
dotnet build -c Release -f netcoreapp2.0
dotnet publish -c Release -r win10-x64