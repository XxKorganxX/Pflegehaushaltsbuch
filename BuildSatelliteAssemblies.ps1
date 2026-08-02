param(
    [Parameter(Mandatory = $true)]
    [string] $IntermediateOutputPath,

    [Parameter(Mandatory = $true)]
    [string] $TargetName,

    [Parameter(Mandatory = $true)]
    [string] $AlToolPath
)

$ErrorActionPreference = 'Stop'

$intermediatePath = Resolve-Path -LiteralPath $IntermediateOutputPath
$targetAssemblyPath = Join-Path $intermediatePath "$TargetName.exe"
$targetAssemblyVersion = [Reflection.AssemblyName]::GetAssemblyName($targetAssemblyPath).Version.ToString()

$cultureNames = Get-ChildItem -LiteralPath $intermediatePath -Filter "*.resources" |
    ForEach-Object {
        if ($_.BaseName -match '\.([a-z]{2,3}(?:-[A-Za-z0-9]+)*)$') {
            try {
                [System.Globalization.CultureInfo]::GetCultureInfo($Matches[1]).Name
            }
            catch {
                $null
            }
        }
    } |
    Where-Object { $_ } |
    Sort-Object -Unique

foreach ($targetCulture in $cultureNames) {
    $resourceFiles = Get-ChildItem -LiteralPath $intermediatePath -Filter "*.$targetCulture.resources"
    if ($resourceFiles.Count -eq 0) {
        continue
    }

    $cultureOutputPath = Join-Path $intermediatePath $targetCulture
    New-Item -ItemType Directory -Path $cultureOutputPath -Force | Out-Null

    $outputAssembly = Join-Path $cultureOutputPath "$TargetName.resources.dll"
    $arguments = @('/target:lib', "/culture:$targetCulture", "/version:$targetAssemblyVersion", "/out:$outputAssembly")

    foreach ($resourceFile in $resourceFiles) {
        $arguments += "/embed:$($resourceFile.FullName),$($resourceFile.Name)"
    }

    & $AlToolPath @arguments

    if ($LASTEXITCODE -ne 0) {
        throw "Assembly Linker failed for culture '$targetCulture' with exit code $LASTEXITCODE."
    }
}

