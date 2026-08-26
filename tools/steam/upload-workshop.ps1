# Загрузка/обновление Workshop item через SteamCMD (preview + контент + метаданные).
# SSOT для игры: облако Workshop → папка подписки. Не копировать DLL в workshop\ вручную.
#
#   pwsh .\upload-workshop.ps1
#   pwsh .\upload-workshop.ps1 -SteamUser exclusivecookie
# Или всё сразу: pwsh .\publish.ps1

param(
    [string] $SteamUser = "exclusivecookie",
    [string] $ChangeNote = ""
)

$ErrorActionPreference = "Stop"
$steamcmd = "D:\_Programms\steamcmd\steamcmd.exe"
$vdfTemplate = Join-Path $PSScriptRoot "workshop_item.vdf"
$pkg = "d:\Files\Mods\Quasimorph\package"
$mediaThumb = "d:\Files\Mods\Quasimorph\media\thumbnail.png"
$descFile = "d:\Files\Mods\Quasimorph\media\workshop-description.txt"

if (-not (Test-Path $steamcmd)) { throw "steamcmd not found: $steamcmd" }
if (-not (Test-Path $vdfTemplate)) { throw "vdf not found: $vdfTemplate" }
if (-not (Test-Path $descFile)) { throw "description not found: $descFile" }

if ((Test-Path $mediaThumb) -and (Test-Path $pkg)) {
    Copy-Item $mediaThumb (Join-Path $pkg "thumbnail.png") -Force
}

foreach ($f in @("thumbnail.png", "modmanifest.json", "QM_ReputationOnMissionTooltip.dll")) {
    $p = Join-Path $pkg $f
    if (-not (Test-Path $p)) { throw "Missing in package\: $f - сначала: dotnet build -c Release" }
}

# SteamCMD заливает \n как текст; в VDF нужны настоящие переводы строк.
$desc = [System.IO.File]::ReadAllText($descFile).TrimEnd()
$desc = $desc -replace '\\', '\\\\' -replace '"', '\"'
if ([string]::IsNullOrWhiteSpace($ChangeNote)) {
    $ChangeNote = "Restore bilingual Workshop description (RU/EN with [hr])."
}
$changeEscaped = $ChangeNote -replace '\\', '\\\\' -replace '"', '\"'

$vdfPath = Join-Path $env:TEMP "qm_reputationonmissiontooltip_workshop.vdf"
$vdf = @"
"workshopitem"
{
	"appid"		"2059170"
	"publishedfileid"	"3790325906"
	"contentfolder"		"d:\\Files\\Mods\\Quasimorph\\package"
	"previewfile"		"d:\\Files\\Mods\\Quasimorph\\package\\thumbnail.png"
	"visibility"		"0"
	"title"			"Reputation on Mission Tooltip"
	"description"		"$desc"
	"changenote"		"$changeEscaped"
}
"@
[System.IO.File]::WriteAllText($vdfPath, $vdf, [System.Text.UTF8Encoding]::new($false))

Write-Host "Steam user: $SteamUser"
Write-Host "VDF: $vdfPath"
Write-Host "Content: $pkg"
Write-Host "Description: $descFile"
& $steamcmd +login $SteamUser +workshop_build_item $vdfPath +quit
exit $LASTEXITCODE
