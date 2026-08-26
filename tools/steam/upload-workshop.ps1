# Загрузка/обновление Workshop item через SteamCMD (preview + контент + метаданные).
# Использование:
#   pwsh .\upload-workshop.ps1 -SteamUser YOUR_STEAM_LOGIN
# Пароль: интерактивно спросит SteamCMD (Steam Guard — в том же окне).

param(
    [Parameter(Mandatory = $true)]
    [string] $SteamUser
)

$ErrorActionPreference = "Stop"
$steamcmd = "D:\_Programms\steamcmd\steamcmd.exe"
$vdf = Join-Path $PSScriptRoot "workshop_item.vdf"

if (-not (Test-Path $steamcmd)) { throw "steamcmd not found: $steamcmd" }
if (-not (Test-Path $vdf)) { throw "vdf not found: $vdf" }
if (-not (Test-Path "d:\Files\Mods\Quasimorph\package\thumbnail.png")) {
    throw "thumbnail.png missing in package\"
}
if (-not (Test-Path "d:\Files\Mods\Quasimorph\package\modmanifest.json")) {
    throw "modmanifest.json missing in package\"
}

Write-Host "Steam user: $SteamUser"
Write-Host "VDF: $vdf"
& $steamcmd +login $SteamUser +workshop_build_item $vdf +quit
exit $LASTEXITCODE
