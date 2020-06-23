docker run `
  -p 5000:80 `
  --name acme `
  --rm `
  -d `
  --network demo `
  jamesworld/acme:1.0

Start-Sleep -Seconds 1
Start-Process "http://localhost:5000/metrics"
Write-Host "Press any key to stop demo"
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

docker stop acme
