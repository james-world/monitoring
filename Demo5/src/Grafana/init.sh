#!/bin/bash

grafana-server \
    --homepath="$GF_PATHS_HOME" \
    --config="$GF_PATHS_CONFIG" \
    cfg:default.paths.data="$GF_PATHS_DATA" &
sleep 10 &&

curl \
    -XPOST \
    -H "Content-Type: application/json" \
    -d '{ "name": "viewer", "email": "viewer@org.com", "login":"viewer", "password":"readonly" }' \
    http://admin:admin@localhost:3000/api/admin/users

curl \
    -X PUT \
    -H 'Content-Type: application/json' \
    -d '{ "homeDashboardId":1 }' \
    http://viewer:readonly@localhost:3000/api/user/preferences \
