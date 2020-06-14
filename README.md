# Monitoring and Metrics

Demos accompanying the brown bag talk of the same name.

## Demo1 - Installing and Running Prometheus

The easiest way to get Prometheus up and running is with it's maintained docker image `prom/prometheus`. The image looks for a YAML configuration file at `/etc/prometheus.yml` and stores it's time series database at `/prometheus`.

This first demo runs up a docker container with local bind mounts to map a simple configuration file and folder for the database.

The minimal configuration sets a global time interval for scraping jobs, and a single job to scrape prometheus itself.
