Write-Host "This packages assumes that the following environment variables are declared:
* TfsUrl (like 'http://tfsurl:8080/tfs')
* TfsLoginName (With AD prefix if necessary)
* TfsPassword (unencrypted, will have to fix that)

Do you want to define these now?"

$answer = Read-Host "yes or no"

while("yes","no" -notcontains $answer)
{
	$answer = Read-Host "yes or no"
}

if($answer -notcontains "no")
{
    [Environment]::SetEnvironmentVariable("TfsUrl", (Read-Host -Prompt 'TfsUrl ("http://tfsurl:8080/tfs")'), "Machine")
    [Environment]::SetEnvironmentVariable("TfsLoginName", (Read-Host -Prompt TfsLoginName), "Machine")
    [Environment]::SetEnvironmentVariable("TfsPassword", (Read-Host -Prompt TfsPassword), "Machine")
}