# Monitoring and Metrics

**An updated demo can be found in the james-world/monitoring-demo repo - leaving this here to preserve context from recorded demos, but the new repo uses docker-compose and is much easier to run up.**

Demos accompanying the brown bag talk of the same name.

## Demo1 - Installing and Running Prometheus

The easiest way to get Prometheus up and running is with it's maintained docker image `prom/prometheus`. The image looks for a YAML configuration file at `/etc/prometheus.yml` and stores it's time series database (tsdb) at `/prometheus`.

This first demo runs up a docker container with local bind mounts to map a simple configuration file and folder for the database.

The minimal configuration sets a global time interval for scraping jobs, and a single job to scrape prometheus itself.

Execute `.\run_demo.ps1` from a powershell console to fire up a prometheus container and open a browser an the prometheus UI.

## Demo2 - Installing and Running Prometheus in Kubernetes

Here we use a pod spec that uses an emptyDir volume mount for the tsdb. The emptyDir volume type is a folder provisioned in local node storage which is deleted when the pod is destoyed. The configuration is passed via a config map.

Prerequisits: kubectl is installed and pointing to a valid cluster (minikube/docker desktop is fine)

To configure this from the Demo2 folder run:

    kubectl create configmap prometheus-config --from-file=prometheus.yml

Then create the pod with:

    k apply -f prometheus_pod.yaml

In order to test everything is working, port forward using

    kubectl port-forward prometheus 9123:9090

and open <http://localhost:9123>.

### Updating the config map

Use this trick to update a configmap created from a file:

```
kubectl create configmap prometheus-config --from-file=prometheus.yml --dry-run -o yaml | kubectl apply -f -
```

Then connect to the prometheus server and send a SIGHUP to the prometheus process to get it to pick up the new config - wait for a minute, because it takes a while for the configmap change to be reflected in the pod (this is a kubernetes limitation).

```
kubectl exec -it prometheus sh
kill -1 1
exit
```

## Demo3 - Adding a Pod that publishes metrics

Prerequisits: kubectl is installed and pointing to a valid cluster (minikube/docker desktop is fine)

The Demo3 folder contains a simple DotNet web api project - it's the standard starter template created with `dotnet new webapi`.

To this project the following additions have been made:

- The prometheus client has been added with `dotnet add package prometheus-net.AspNetCore`.
- A standard asp dotnet core multistage Dockerfile has been created, and an environment variable
  `ENV ASPNETCORE_URLS http://*:80` added publish on the public networks (instead of just localhost)
- A kube pod spec has been added (this has an annotation we'll come back to later)

The startup has been modified as the comments below indicate:

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();

    app.UseRouting();
    // capture http metrics
    app.UseHttpMetrics();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        // expose /metrics endpoint
        endpoints.MapMetrics();
    });
}
```

The `WeatherForecastController.cs` has been altered to capture a Counter metric.

Build the docker container:

- `docker build -t jamesworld\monitoring-dotnetsample:1.0 .\Demo3\src\DotNetSample\.`

## Demo 4 - A slightly more serious Kuberbetes deployment

Using a monitoring namespace, cluster role and scrape configs.

- `prometheus_namespace.yaml` defines a monitoring namespace and cluster role and cluster role binding for the prometheus service. Deploy with `kubectl apply -f prometheus_namespace.yaml`.
- Updated configuration in `prometheus.yml` ensures only pods carrying the `prometheus.io/scrape` annotation are collected. Deploy this with `kubectl crete configmap prometheus-config -n monitoring --from-file=prometheus.yml --dry-run -o yaml | kubectl apply -f -`
- `promethues_deployment.yaml` is a deployment of the kube pod into the monitoring namespace. `kubectl apply -f prometheus_deployment.yaml` takes care of that.

## Demo 5 - The Stock App, with Prometheus and Grafana

Includes:

- A simple `ASP.NET Core 3.1` app that makes use of the `prometheus-net.AspNetCore` library to expose a metrics endpoint and a custom `product_stock_levels` Prometheus Gauge
- A grafana instance with pre-configured dashboards drawing from a prometheus instance.
- A jmeter load test to exercise the app.

Prerequists:

- To run the load tests, jmeter should be installed
- To use the `SampleApiCalls.rest` to send manual requests, include the Visual Studio Code `REST Client` plugin.
- Docker is installed
- Create a local named docker network with `docker network create demo`

Build the docker containers:

- `docker build -t jamesworld\acme:1.0 .\Demo5\src\Acme\.`
- `docker build -t jamesworld\acme:1.0 .\Demo5\src\Grafana\.`

Run `.\Demo5\run_demo.ps1` from a powershell prompt to start the docker containers
