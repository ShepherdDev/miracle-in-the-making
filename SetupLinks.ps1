<# Right-click in Explorer and select 'Run In Powershell'
 #
 # - Version 1.0:
 # Builds a hard link from the main project folder (containing this script) to the RockIt folder.
 # Builds a hard link from the optional Controls folder to the 
 #>


<#
 # Ask the user for a folder.
 #>
Function Select-FolderDialog
{
    param([string]$Description="Select Folder",[string]$RootFolder="Desktop")

	[System.Reflection.Assembly]::LoadWithPartialName("System.windows.forms") | Out-Null     

	$objForm = New-Object System.Windows.Forms.FolderBrowserDialog
    $objForm.Rootfolder = $RootFolder
    $objForm.Description = $Description
    $Show = $objForm.ShowDialog()
    If ($Show -eq "OK")
    {
        Return $objForm.SelectedPath
    }
    Else
    {
        Write-Error "Operation cancelled by user."
    }
}

<#
 # Ask the user where their RockIt folder is.
 #>
$RockItPath = Select-FolderDialog("Select the RockIt folder")
$RockWebPath = Join-Path $RockItPath "RockWeb"

<#
 # Get some helpful variables for path references.
 #>
$ProjectPath = Split-Path (Get-Variable MyInvocation).Value.MyCommand.Path
$ProjectControlsPath = Join-Path $ProjectPath "Controls"
$ProjectThemesPath = Join-Path $ProjectPath "Themes"
$ProjectFullName = (Get-ChildItem -Path $ProjectPath -Filter *.csproj)[0].Name
$ProjectFullName = $ProjectFullName.Substring(0, $ProjectFullName.Length - 7)
$ProjectOrganziation = $ProjectFullName.Substring(0, $ProjectFullName.LastIndexOf('.'))
$ProjectName = $ProjectFullName.Substring($ProjectFullName.LastIndexOf('.') + 1)
$RockItPluginsPath = Join-Path $RockWebPath "Plugins"
$RockItThemesPath = Join-Path $RockWebPath "Themes"
$RockItPluginOrganizationPath = Join-Path $RockItPluginsPath $ProjectOrganziation.Replace(".", "_")
$RockItPluginProjectPath = Join-Path $RockItPluginOrganizationPath $ProjectName

<#
 # Make sure this is a RockIt path.
 #>
if ( !(Test-Path $RockItPluginsPath) -or !(Test-Path $RockItThemesPath) )
{
	throw "Path does not appear to be a valid RockIt path"
}

<#
 # Create any intermediate folders we need.
 #>
if ( !(Test-Path $RockItPluginOrganizationPath) )
{
	Write-Host "Creating"
	Write-Host $RockItPluginOrganizationPath
	New-Item -Path $RockItPluginsPath -Name $ProjectOrganziation.Replace(".", "_") -ItemType directory
}

<#
 # Hard link the a from the Plugins folder to the Project Controls.
 #>
if ( Test-Path $ProjectControlsPath )
{
	if ( !(Test-Path $RockItPluginProjectPath) )
	{
		cmd /c mklink /J "$RockItPluginProjectPath" "$ProjectControlsPath"
	}
}

<#
 # Hard link each theme if it doesn't already exist.
 #>
if ( Test-Path $ProjectThemesPath )
{
	$themes = (Get-ChildItem -Path $ProjectThemesPath)
	Foreach ($theme in $themes)
	{
		$SourceTheme = Join-Path $ProjectThemesPath $theme
		$TargetTheme = Join-Path $RockItThemesPath $theme

		if ( !(Test-Path $TargetTheme) )
		{
			cmd /c mklink /J "$TargetTheme" "$SourceTheme"
		}
	}
}

<#
 # Hard link the actual project path.
 #>
$TargetProjectPath = Join-Path $RockItPath $ProjectFullName
if ( !(Test-Path $TargetProjectPath) )
{
	Write-Host $TargetProjectPath
	Write-Host $ProjectPath
	cmd /c mklink /J "$TargetProjectPath" "$ProjectPath"
}

