docker run `
  -p 9090:9090 `
  -v $psscriptroot\prometheus.yml:/etc/prometheus/prometheus.yml `
  -v $psscriptroot\PrometheusVol:/prometheus `
  --name prometheus `
  --rm `
  -d `
  --network demo `
  prom/prometheus

docker run `
  -p 5000:80 `
  --name acme `
  --rm `
  -d `
  --network demo `
  jamesworld/acme:1.0

Start-Process "http://localhost:9090"
Write-Host "Press any key to stop demo"
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

docker stop -t 5 prometheus
docker stop acme
Remove-Item -Force -Recurse "$psscriptroot\PrometheusVol"
