docker run `
  -p 9090:9090 `
  -v $psscriptroot\prometheus.yml:/etc/prometheus/prometheus.yml `
  -v $psscriptroot\PrometheusVol:/prometheus `
  --name prometheus `
  --rm `
  -d `
  prom/prometheus

Start-Process "http://localhost:9090"
Write-Host "Press any key to stop prometheus"
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

docker stop -t 5 prometheus
Remove-Item -Force -Recurse "$psscriptroot\PrometheusVol"
