[Environment]::SetEnvironmentVariable("TfsUrl", "http://tfsurl:8080/tfs", "User")
[Environment]::SetEnvironmentVariable("TfsLoginName", (Read-Host -Prompt TfsLoginName), "User")
[Environment]::SetEnvironmentVariable("TfsPassword", (Read-Host -Prompt TfsPassword), "User")
[Environment]::SetEnvironmentVariable("TfsExtractPath", (Read-Host -Prompt TfsExtractPath), "User")
# Filename prefix
[Environment]::SetEnvironmentVariable("TfsExtractPrefix", (Read-Host -Prompt TfsExtractPrefix), "User")

# Filename without extension
[Environment]::SetEnvironmentVariable("TfsExtractProjectList", (Read-Host -Prompt TfsExtractProjectList), "User")
[Environment]::SetEnvironmentVariable("TfsExtractMachineList", (Read-Host -Prompt TfsExtractMachineList), "User")
[Environment]::SetEnvironmentVariable("TfsExtractUsersList", (Read-Host -Prompt TfsExtractUsersList), "User")

# In case of OU extract from Active Directory
[Environment]::SetEnvironmentVariable("LDAPAddress", (Read-Host -Prompt LDAPAddress), "User")
[Environment]::SetEnvironmentVariable("LDAPLogin", (Read-Host -Prompt LDAPLogin), "User")
[Environment]::SetEnvironmentVariable("LDAPPassword", (Read-Host -Prompt LDAPPassword), "User")
[Environment]::SetEnvironmentVariable("LDAP_OU_FILTER_OUT", (Read-Host -Prompt LDAP_OU_FILTER_OUT), "User")

# In case of SFTP upload of the JSON extracts
[Environment]::SetEnvironmentVariable("TfsExtractSSH_Host", (Read-Host -Prompt TfsExtractSSH_Host), "User")
[Environment]::SetEnvironmentVariable("TfsExtractSSH_User", (Read-Host -Prompt TfsExtractSSH_Host), "User")
[Environment]::SetEnvironmentVariable("TfsExtractSSH_Password", (Read-Host -Prompt TfsExtractSSH_Host), "User")
[Environment]::SetEnvironmentVariable("TfsExtractSSH_Path", (Read-Host -Prompt TfsExtractSSH_Host), "User")



