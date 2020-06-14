# Monitoring and Metrics

Demos accompanying the brown bag talk of the same name.

## Demo1 - Installing and Running Prometheus

The easiest way to get Prometheus up and running is with it's maintained docker image `prom/prometheus`. The image looks for a YAML configuration file at `/etc/prometheus.yml` and stores it's time series database (tsdb) at `/prometheus`.

This first demo runs up a docker container with local bind mounts to map a simple configuration file and folder for the database.

The minimal configuration sets a global time interval for scraping jobs, and a single job to scrape prometheus itself.

## Demo2 - Installing and Running Prometheus in Kubernetes

Here we use a pod spec that uses an emptyDir volume mount for the tsdb. The emptyDir volume type is a folder provisioned in local node storage which is deleted when the pod is destoyed. The configuration is passed via a config map.

To configure this from the Demo2 folder run:

    kubectl create configmap prometheus-config --from-file=prometheus.yml

Then create the pod with:

    k apply -f prometheus_pod.yaml

In order to test everything is working, port forward using

    kubectl port-forward prometheus 9123:9090

and open <http://localhost:9123>.
