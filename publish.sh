#!/bin/sh
CONF="Debug"
echo $# arguments 
if [ $# == 1 ] || [ $# == 2 ]
then
    echo "Assembly name $1"

    if [ $# == 2 ]
    then
        if [ $2 == "Debug" ] || [ $2 == "Release" ]
        then
            CONF=$2
        fi
    else
        echo "Configuration unkonwn: selecting Debug"        
    fi

    echo "calling build $1 $CONF"
    ./build.sh $1 $CONF

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
