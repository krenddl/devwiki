#!/bin/bash
dotnet run --project DevWiki.API &
API_PID=$!
sleep 5

curl -v -X POST -H "Content-Type: application/json" -d '{"Username":"admin","Password":"123"}' http://localhost:5285/api/Auth/login 2>&1 | grep "Set-Cookie"

kill $API_PID
