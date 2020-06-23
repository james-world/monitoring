Start-Process "https://localhost:5001/metrics"

& dotnet run --project "$PSScriptRoot\src\Acme\Acme.csproj"
