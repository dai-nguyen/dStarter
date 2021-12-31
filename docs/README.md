connectionstring is in user secret
%appdata%\Microsoft\UserSecrets\aspnet-Web-3894A604-EB10-4369-BDC8-F3E240AABC9C

create self-signed cert in powershell:
$cert = New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dns my-k8s.io
$pwd = ConvertTo-SecureString -String "Secret" -Force -AsPlainText
$certpath = "Cert:\localmachine\my\$($cert.Thumbprint)"
Export-PfxCertificate -Cert $certpath -FilePath c:\Web.pfx -Password $pwd

add-migration
update-database

https://github.com/openiddict/openiddict-core/tree/dev/samples/Mvc.Server
