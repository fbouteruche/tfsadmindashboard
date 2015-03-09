[Environment]::SetEnvironmentVariable("TfsUrl", "http://tfsurl:8080/tfs", "User")
[Environment]::SetEnvironmentVariable("TfsLoginName", (Read-Host -Prompt TfsLoginName), "User")
[Environment]::SetEnvironmentVariable("TfsPassword", (Read-Host  -Prompt TfsPassword), "User")

# Optional domain extension for the rdps connection (also thick to use the TfsAdminDashboardPS\rdp.bat to register the rdp protocol)
[Environment]::SetEnvironmentVariable("RDPDomain", ".domain.com", "User")