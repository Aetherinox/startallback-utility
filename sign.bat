@echo OFF

set CERT_THUMBPRINT=58a1539d6988d76f44bae27c27ed5645d3b1222a

for %%a in (%*) do call /signtool/signtool sign /sha1 %CERT_THUMBPRINT% /fd SHA256 /t http://timestamp.comodoca.com/authenticode "%%a"

echo.
echo Press enter to Quit
set /p input=