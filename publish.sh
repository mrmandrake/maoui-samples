#!/bin/sh
echo $# arguments 
if [ $# == 3 ]
then
    echo "Assembly name $1"
    CONF="Debug"
    if [ $2 != "Debug" ] && [ $2 != "Release" ]
    then
        echo "Configuration unkonwn: selecting Debug"
    else
        CONF=$2
    fi

    echo "calling build $1 $CONF $3"
    ./build.sh $1 $CONF $3

    echo "clearing chrome cache..."
    rm -R ~/Library/Caches/Google/Chrome/Default/Cache

    echo "clearing teka previous build"
    rm -rvf ../maoui/teka/publish
    rm -rvf ../maoui/teka/wwwroot
    echo "moving $1 in publish wwwroot"
    mv ./$1/bin/$CONF/netstandard2.1/publish ../maoui/teka/
    mv ../maoui/teka/publish ../maoui/teka/wwwroot
    cd ../maoui/Teka
    echo running Teka $1
    dotnet run $1 
else
    echo "illegal number of parameters"
    echo "usage: build.sh <assembly> <Debug|Release> [mt|nomt]"
fi