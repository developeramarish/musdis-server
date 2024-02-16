# Script for adding migrations 

[CmdletBinding()]
param (
    [Alias("sn")]
    [Parameter()]
    [string]
    $ServiceName
)

dotnet ef database update --project .\src\Services\$ServiceName\ --msbuildprojectextensionspath .\artifacts\obj\$ServiceName\