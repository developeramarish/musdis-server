# 📦 Packages 
Contains NuGet packages for microservices. 

## 📃 List of packages 

- _[OperationResults](../src/Common/OperationResults/README.md)_ - package for using Operation result pattern in C# code.

- _[ResponseHelpers](../src/Common/ResponseHelpers/README.md)_ - package that contains useful response models.

## 📝 Get started 

To enable this packages in your projects, do the following: 

### Add existing packages 

1. Add NuGet source (should be added only once)

```shell
nuget sources add -Name "Musdis" -Source {your-path}/musdis-server/packages
```

2. Add packages to projects

```sh
dotnet add {project-path.csproj} package {package-name} -v {version} -s {your-path}/musdis-server/packages
```

### Add own packages

1. Pack packages from _src/Common_ directory 

dotnet CLI: 
```sh
dotnet pack ~/path/to/package/project.csproj 
```

2. Add NuGet source (should be added only once)

```shell
nuget sources add -Name "Musdis" -Source {your-path}/musdis-server/packages
```

3. Add packages to NuGet source
```sh
nuget add {your-path}/musdis-server/artifacts/package/OperationResults/release/{package-name}.nupkg -Source {your-path}/musdis-server/packages
```

4. [Add NuGet packages to projects](#add-existing-packages).
