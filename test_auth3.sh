#!/bin/bash
dotnet run --project DevWiki.API > api.log 2>&1 &
API_PID=$!
sleep 5

echo "Registering testuser1"
curl -s -X POST -H "Content-Type: application/json" -d '{"Username":"testuser1","Password":"testpassword123"}' http://localhost:5285/api/Auth/register

echo -e "\nLogging in testuser1"
curl -s -i -X POST -H "Content-Type: application/json" -d '{"Username":"testuser1","Password":"testpassword123"}' http://localhost:5285/api/Auth/login

echo -e "\nLogging in with wrong password"
curl -s -i -X POST -H "Content-Type: application/json" -d '{"Username":"testuser1","Password":"wrongpassword"}' http://localhost:5285/api/Auth/login

kill $API_PID
