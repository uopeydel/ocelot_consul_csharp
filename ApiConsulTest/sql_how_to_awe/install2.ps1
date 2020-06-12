domain\administrator

Install-WindowsFeature -Name HostGuardianServiceRole -IncludeManagementTools -Restart 

$AdminPassword = ConvertTo-SecureString -AsPlainText 'tmhctr!234' -Force
Install-HgsServer -HgsDomainName 'domain.local' -SafeModeAdministratorPassword $AdminPassword -Restart

Initialize-HgsAttestation -HgsServiceName 'hgs' -TrustHostKey 
------------------------------http://172.17.117.243/
Enable-WindowsOptionalFeature -Online -FeatureName HostGuardian -All

Set-ItemProperty -Path HKLM:\SYSTEM\CurrentControlSet\Control\DeviceGuard -Name RequirePlatformSecurityFeatures -Value 0 

Set-HgsClientHostKeynt
Get-HgsClientHostKey -Path $HOME\Desktop\hostkey.cer 
Add-HgsAttestationHostKey -Name SQLSERVER2019 -Path $HOME\Desktop\hostkey.cer

Set-HgsClientConfiguration -AttestationServerUrl http://172.17.117.243/Attestation -KeyProtectionServerUrl http://172.17.117.243/KeyProtection/