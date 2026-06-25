#!/bin/bash
dotnet run --project DevWiki.API > api5.log 2>&1 &
API_PID=$!
while ! grep -q "Now listening on" api5.log; do
  sleep 1
done

echo "Registering testuser5"
curl -s -X POST -H "Content-Type: application/json" -d '{"Username":"testuser5","Password":"testpassword123"}' http://localhost:5285/api/Auth/register
echo ""

echo "Logging in testuser5"
LOGIN_RESP=$(curl -s -X POST -H "Content-Type: application/json" -d '{"Username":"testuser5","Password":"testpassword123"}' http://localhost:5285/api/Auth/login)
echo "Login Response: $LOGIN_RESP"

TOKEN=$(echo $LOGIN_RESP | jq -r '.token')
echo "Extracted Token: $TOKEN"

echo "Calling /me"
curl -s -i -H "Authorization: Bearer $TOKEN" http://localhost:5285/api/Auth/me

kill $API_PID
