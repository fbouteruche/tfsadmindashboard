[Environment]::SetEnvironmentVariable("TfsUrl", (Read-Host -Prompt 'TfsUrl ("http://tfsurl:8080/tfs")'), "Machine")
[Environment]::SetEnvironmentVariable("TfsLoginName", (Read-Host -Prompt TfsLoginName), "Machine")
[Environment]::SetEnvironmentVariable("TfsPassword", (Read-Host -Prompt TfsPassword), "Machine")