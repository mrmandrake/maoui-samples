#!/bin/sh
kill -9 $(ps ax | grep 'python server.py' | awk '{print $1}')
kill -9 $(ps ax | grep 'ProxyDriver.dll' | awk '{print $1}')