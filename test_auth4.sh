#!/bin/bash
dotnet run --project DevWiki.API > api4.log 2>&1 &
API_PID=$!
# Wait for API to be ready
while ! grep -q "Now listening on" api4.log; do
  sleep 1
done

echo "Registering testuser2"
curl -s -w "\nHTTP_STATUS:%{http_code}\n" -X POST -H "Content-Type: application/json" -d '{"Username":"testuser2","Password":"testpassword123"}' http://localhost:5285/api/Auth/register

echo -e "\nLogging in testuser2"
curl -s -i -w "\nHTTP_STATUS:%{http_code}\n" -X POST -H "Content-Type: application/json" -d '{"Username":"testuser2","Password":"testpassword123"}' http://localhost:5285/api/Auth/login

kill $API_PID
