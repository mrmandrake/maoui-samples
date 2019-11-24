#!/bin/bash
echo $# arguments 
if [ $# == 3 ]
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
        cp -v ../../../../../maoui/netstandard2.1/*.dll ./bin/$2/netstandard2.1
        cp -v ../../../../../maoui/netstandard2.1/*.pdb ./bin/$2/netstandard2.1
        cd ./bin/$2/netstandard2.1
        echo "moved in "
        pwd
        if [ $3 == "mt" ]
        then
            echo "multithread build..."
            if [ $2 == "Debug" ]
            then
                mono ../mono-wasm/packager.exe --copy=always --out=./publish -debug $1.dll --threads
            else
                mono ../mono-wasm/packager.exe --copy=always --out=./publish $1.dll --threads
            fi

        else
            echo "NO multithread"
            if [ $2 == "Debug" ]
            then
                mono ../mono-wasm/packager.exe --copy=always --out=./publish -debug $1.dll
            else
                mono ../mono-wasm/packager.exe --copy=always --out=./publish $1.dll
            fi
        fi

        mkdir ./publish/
        mkdir ./publish/css
        mkdir ./publish/js
        cp -rvf ../../../../../maoui/assets/*.* ./publish/
        cp -rvf ../../../../../maoui/assets/css/*.* ./publish/css/
        cp -rvf ../../../../../maoui/assets/js/*.* ./publish/
    else
        echo "Configuration unknown: selecting Debug"
        $2 = "Debug"
    fi
else
    echo "illegal number of parameters"
    echo "usage: build.sh <assembly> <Debug|Release> [mt|nomt]"
fi