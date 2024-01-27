# Packages 📦
Contains NuGet packages for microservices. 

## List of packages 📃

- _[OperationResults](../src/Common/OperationResults/README.md)_ - package for using Operation result pattern in C# code.

## Get started 

To enable this packages in your projects, do the following: 

### Simple 

1. Add nuget source

```shell
nuget sources add -Name "Musdis" -Source {your-path}/musdis-server/packages
```

2. Add packages to projects

```sh
dotnet add {project-path.csproj} package {package-name} -v {version} -s {your-path}/musdis-server/packages
```

### Complicated

1. Pack packages from _src/Common_ directory 

dotnet CLI: 
```sh
dotnet pack ~/path/to/package/project.csproj 
```

2. Add packages to NuGet source
```sh
nuget add {your-path}/musdis-server/bin/Results/Release/{package-name}.nupkg -Source {your-path}/musdis-server/packages
```

3. Add NuGet source and packages to projects following instructions [above](#simple).
