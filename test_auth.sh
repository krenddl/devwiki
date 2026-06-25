#!/bin/bash
# Start API in background
dotnet run --project DevWiki.API &
API_PID=$!
sleep 5

# Register
curl -s -X POST -H "Content-Type: application/json" -d '{"Username":"testuser_new","Password":"testpassword123"}' http://localhost:5285/api/Auth/register
echo ""
# Login
curl -s -X POST -H "Content-Type: application/json" -d '{"Username":"testuser_new","Password":"testpassword123"}' http://localhost:5285/api/Auth/login
echo ""

kill $API_PID
