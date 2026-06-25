#!/bin/bash
dotnet run --project DevWiki.API &
API_PID=$!
sleep 5

curl -i -X POST -H "Content-Type: application/json" -d '{"Username":"admin","Password":"123"}' http://localhost:5285/api/Auth/login

kill $API_PID
