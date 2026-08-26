# Сборка package\ + загрузка в Steam Workshop (SSOT для игры при подписке на свой мод).
#   pwsh .\publish.ps1
#   pwsh .\publish.ps1 -SteamUser exclusivecookie

param(
    [string] $SteamUser = "exclusivecookie"
)

$ErrorActionPreference = "Stop"
$root = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
if (-not (Test-Path (Join-Path $root "modmanifest.json"))) {
    $root = "d:\Files\Mods\Quasimorph"
}

Set-Location (Join-Path $root "src")
dotnet build QM_ReputationOnMissionTooltip.csproj -c Release
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$pkg = Join-Path $root "package"
foreach ($f in @("QM_ReputationOnMissionTooltip.dll", "modmanifest.json", "thumbnail.png")) {
    $p = Join-Path $pkg $f
    if (-not (Test-Path $p)) { throw "Missing in package: $f" }
    Write-Host "OK $f ($((Get-Item $p).Length) bytes)"
}

Set-Location (Join-Path $root "tools\steam")
& (Join-Path $PSScriptRoot "upload-workshop.ps1") -SteamUser $SteamUser
exit $LASTEXITCODE
