#!/bin/bash
dotnet run --project DevWiki.API > api6.log 2>&1 &
API_PID=$!
while ! grep -q "Now listening on" api6.log; do
  sleep 1
done

LOGIN_RESP=$(curl -s -X POST -H "Content-Type: application/json" -d '{"Username":"testuser5","Password":"testpassword123"}' http://localhost:5285/api/Auth/login)
TOKEN=$(echo $LOGIN_RESP | grep -o '"token":"[^"]*' | grep -o '[^"]*$')

curl -s -i -H "Authorization: Bearer $TOKEN" http://localhost:5285/api/Auth/me > test_me.log

kill $API_PID
