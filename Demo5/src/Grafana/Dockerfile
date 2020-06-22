# Should provide a verison here
FROM grafana/grafana
USER root
RUN apk add --no-cache curl

USER grafana

COPY datasource-prometheus.yaml /etc/grafana/provisioning/datasources/
COPY dashboard-provider.yaml /etc/grafana/provisioning/dashboards/
COPY prometheus-dashboard.json /var/lib/grafana/dashboards/
COPY acme-dashboard.json /var/lib/grafana/dashboards/

COPY init.sh .
RUN ./init.sh
