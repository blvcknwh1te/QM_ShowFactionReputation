# Загрузка/обновление Workshop item через SteamCMD (preview + контент + метаданные).
# SSOT для игры: облако Workshop → папка подписки. Не копировать DLL в workshop\ вручную.
#
#   pwsh .\upload-workshop.ps1
#   pwsh .\upload-workshop.ps1 -SteamUser exclusivecookie
# Или всё сразу: pwsh .\publish.ps1

param(
    [string] $SteamUser = "exclusivecookie"
)

$ErrorActionPreference = "Stop"
$steamcmd = "D:\_Programms\steamcmd\steamcmd.exe"
$vdf = Join-Path $PSScriptRoot "workshop_item.vdf"
$pkg = "d:\Files\Mods\Quasimorph\package"
$mediaThumb = "d:\Files\Mods\Quasimorph\media\thumbnail.png"

if (-not (Test-Path $steamcmd)) { throw "steamcmd not found: $steamcmd" }
if (-not (Test-Path $vdf)) { throw "vdf not found: $vdf" }

if ((Test-Path $mediaThumb) -and (Test-Path $pkg)) {
    Copy-Item $mediaThumb (Join-Path $pkg "thumbnail.png") -Force
}

foreach ($f in @("thumbnail.png", "modmanifest.json", "QM_ShowFactionReputation.dll")) {
    $p = Join-Path $pkg $f
    if (-not (Test-Path $p)) { throw "Missing in package\: $f — сначала: dotnet build -c Release" }
}

Write-Host "Steam user: $SteamUser"
Write-Host "VDF: $vdf"
Write-Host "Content: $pkg"
& $steamcmd +login $SteamUser +workshop_build_item $vdf +quit
exit $LASTEXITCODE
