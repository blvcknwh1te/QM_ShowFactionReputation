# Копирует игровые DLL в libs/ мода (после git clone, до dotnet build).
#   pwsh .\tools\setup-libs.ps1
#   pwsh .\tools\setup-libs.ps1 -GameRoot "D:\_Games\SteamGames\steamapps\common\Quasimorph"

param(
    [string] $GameRoot = "D:\_Games\SteamGames\steamapps\common\Quasimorph",
    [string] $PublicizedSource = ""
)

$ErrorActionPreference = "Stop"
$modRoot = Split-Path $PSScriptRoot -Parent
$managed = Join-Path $GameRoot "Quasimorph_Data\Managed"
$gameLibs = Join-Path $modRoot "libs\game"
$publicizedLibs = Join-Path $modRoot "libs\publicized"

if (-not (Test-Path $managed)) {
    throw "Каталог игры не найден: $managed`nУкажи -GameRoot с путём к Quasimorph."
}

$gameDlls = @(
    "0Harmony.dll",
    "UnityEngine.dll",
    "UnityEngine.CoreModule.dll",
    "UnityEngine.UI.dll",
    "Unity.TextMeshPro.dll"
)

New-Item -ItemType Directory -Path $gameLibs -Force | Out-Null
New-Item -ItemType Directory -Path $publicizedLibs -Force | Out-Null

foreach ($name in $gameDlls) {
    $src = Join-Path $managed $name
    if (-not (Test-Path $src)) { throw "Нет в Managed: $name" }
    Copy-Item $src (Join-Path $gameLibs $name) -Force
    Write-Host "OK libs/game/$name"
}

$publicizedDest = Join-Path $publicizedLibs "Assembly-CSharp-publicized.dll"
if (Test-Path $publicizedDest) {
    Write-Host "OK libs/publicized/Assembly-CSharp-publicized.dll (уже есть)"
} elseif ($PublicizedSource -and (Test-Path $PublicizedSource)) {
    Copy-Item $PublicizedSource $publicizedDest -Force
    Write-Host "OK libs/publicized/Assembly-CSharp-publicized.dll (из -PublicizedSource)"
} else {
    throw @"
Нет libs/publicized/Assembly-CSharp-publicized.dll.
Нужен publicized Assembly-CSharp (private-члены доступны для Harmony).
Положи файл в libs/publicized/ вручную или передай:
  pwsh .\tools\setup-libs.ps1 -PublicizedSource 'путь\Assembly-CSharp-publicized.dll'
"@
}

Write-Host "Готово. Дальше: dotnet build src\QM_ReputationOnMissionTooltip.csproj -c Release"
