# Script for adding migrations 

[CmdletBinding()]
param (
    [Alias("sn")]
    [Parameter()]
    [string]
    $ServiceName,

    [Alias("n")]
    [Parameter()]
    [string]
    $Name
)

dotnet ef migrations add $Name -p .\src\Services\$ServiceName\ --msbuildprojectextensionspath .\artifacts\obj\$ServiceName\