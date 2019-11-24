#!/bin/sh
echo $# arguments 
if [$# -ne 3]; then 
    echo "illegal number of parameters"
    echo "usage: build.sh <assembly> <Debug|Release> [mt|nomt]"
fi

if ["$2" != "Debug" && "$2" != "Release"]; then
    echo "Configuration unkonwn: selecting Debug"
    $2 = "Debug"
fi

echo "calling build $1 $2 $3"
./build.sh $1 $2 $3

echo "clearing chrome cache..."
rm -R ~/Library/Caches/Google/Chrome/Default/Cache

echo "updating $1 in publish wwwroot"
cp -rvf ./$1/bin/$2/netstandard2.1/publish/*.* ../maoui/teka/
cp -rvf ../maoui/teka/publish/*.* ../maoui/teka/wwwroot
cd ../maoui/Teka
echo running Teka $1
dotnet run $1 

