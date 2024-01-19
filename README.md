# Musdis 
Backend for music distribution application built with dotnet microservices

## Get started
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

1. Pack packages from src/Common directory 

dotnet CLI: 
```sh
dotnet pack ~/path/to/package/project.csproj 
```

2. Add packages to nuget source
```sh
nuget add {your-path}/musdis-server/bin/Results/Release/{package-name}.nupkg -Source {your-path}/musdis-server/packages
```

3. Add NuGet source and packages to projects from [above](#simple)
