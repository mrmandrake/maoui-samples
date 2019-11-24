#!/bin/bash
echo starting static file server at 127.0.0.1:8000 ...
cd $1/bin/$2/netstandard2.0/publish
python3 server.py&
echo starting mono debugging proxy...
dotnet  ~/mono-wasm/dbg-proxy/netcoreapp3.0/ProxyDriver.dll&
echo starting chrome...
/Applications/Google\ Chrome.app/Contents/MacOS/Google\ Chrome --remote-debugging-port=9222&
/Applications/Google\ Chrome\ Canary.app/Contents/MacOS/Google\ Chrome\ Canary http://localhost:9300&

