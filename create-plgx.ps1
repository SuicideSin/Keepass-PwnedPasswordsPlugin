if(test-path .\PwnedPasswordsPlugin\bin)
{
    Remove-Item -Recurse .\PwnedPasswordsPlugin\bin
}
if(test-path .\PwnedPasswordsPlugin\obj)
{
    Remove-Item -Recurse .\PwnedPasswordsPlugin\obj
}
.\KeePass-2.38\KeePass.exe --plgx-create PwnedPasswordsPlugin --plgx-prereq-net:4.5 --plgx-prereq-kp:2.09