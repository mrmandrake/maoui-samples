#!/bin/bash
export WASM_SDK=/mnt/c/src/mono-wasm
export EMSDK=/mnt/c/src/emsdk
echo $# arguments 
if [ $# == 2 ]
then 
    if [ "$2" == "Debug" ] || [ "$2" == "Release" ]
    then
        cd $1
        dotnet clean
        dotnet restore
        dotnet build -p:Configuration=$2
        echo "current directory:"
        pwd
        cp -v ~/.nuget/packages/xamarin.forms/4.3.0.991211/lib/netstandard2.0/*.dll ./bin/$2/netstandard2.1
        cp -v ~/.nuget/packages/xamarin.forms/4.3.0.991211/lib/netstandard2.0/*.pdb ./bin/$2/netstandard2.1
        cp -v ~/.nuget/packages/newtonsoft.json/12.0.3/lib/netstandard2.0/*.dll ./bin/$2/netstandard2.1
        cd ./bin/$2/netstandard2.1
	rm WebAssembly.bindings.dll
        echo "moved in "
        pwd

        echo "AOT multithread build..."
        if [ $2 == "Debug" ]
        then
		mono $WASM_SDK/packager.exe --emscripten-sdkdir=$EMSDK --mono-sdkdir=$WASM_SDK -appdir=bin_aot --builddir=aot --aot --template=$WASM_SDK/runtime.js --link-mode=SdkOnly --asset=$WASM_SDK/sample.html --asset=$WASM_SDK/server.py --debugrt $1.dll
        else
		mono $WASM_SDK/packager.exe --emscripten-sdkdir=$EMSDK --mono-sdkdir=$WASM_SDK -appdir=bin_aot --builddir=aot --aot --template=$WASM_SDK/runtime.js --link-mode=SdkOnly --asset=$WASM_SDK/sample.html --asset=$WASM_SDK/server.py $1.dll
        fi
    fi
else
    echo "illegal number of parameters"
    echo "usage: build.sh <assembly> <Debug|Release>"
fi
