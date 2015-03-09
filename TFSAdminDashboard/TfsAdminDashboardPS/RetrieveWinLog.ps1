$credential = Get-Credential -Credential "credential"
$desthostname = "hostname"

function Set-TrustedHosts([string]$hostname)
{
    $trustedhosts = Get-Item WSMan:\localhost\Client\TrustedHosts;
    if(!$trustedhosts.Value.Contains($hostname))
    {
        Write-Verbose("Add VM to WinRM TrustedHosts");
        Set-Item WSMan:\localhost\Client\TrustedHosts -Concatenate -Force -Value $hostname;
    }    
}

function Test-Provider ([System.Object[]]$p_providers, [string]$providername )
{
    return  (($p_providers | Where-Object {$_.Name -eq $providername}) -ne $null)
}

if($env:USERDOMAIN -like $env:COMPUTERNAME)
{
    Set-TrustedHosts($desthostname);
}

$providers = Get-WinEvent -ListProvider * -ComputerName $desthostname -Credential $credential

$mvsta = $providers | Where-Object {$_.Name -eq "Microsoft Visual Studio Tools for Applications"}
$reportmanager = $providers | Where-Object {$_.Name -eq "Report Manager (DBTFSREPORTPROD)"}
$tfsservices = $providers | Where-Object {$_.Name -eq "TFS Services"}
$tfsbuildservicehost = $providers | Where-Object {$_.Name -eq "TFSBuildServiceHost"}

$msvstaevt = $null
if($mvsta -ne $null)
{
    $msvstaevt = Get-WinEvent -ProviderName $mvsta.Name -ComputerName $desthostname -Credential $credential
    if($msvstaevt -ne $null)
    {
        Format-Table $msvstaevt
    }
}

$reportmanagerevt = $null
if($reportmanager -ne $null)
{
    $reportmanagerevt = Get-WinEvent -ProviderName $reportmanager.Name -ComputerName $desthostname -Credential $credential
    if($reportmanagerevt -ne $null)
    {
        Format-Table $reportmanagerevt
    }
}

$tfsservicesevt = $null

if(Test-Provider $providers "TFS Services")
{
    $tfsservicesevt = Get-WinEvent -ProviderName $tfsservices.Name -ComputerName $desthostname -Credential $credential -MaxEvents 100
    if($tfsservicesevt -ne $null)
    {
        $tfsservicesevt | Select-Object -Property TimeCreated, Id, LevelDisplayName, LogName, ProviderName, MachineName, Message | Sort-Object -Descending -Property TimeCreated | Out-GridView
        
    }
}