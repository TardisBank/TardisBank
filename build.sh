#! /usr/bin/env bash
set -e

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd)"

if [ $# -gt 1 -a "$1" == "push" ]
then 
    TAG=$2

    echo "Pushing TardisBank ($TAG)"
    echo "-------------------------"

    docker push tardisbank/server:$TAG
    docker push tardisbank/client:$TAG
    docker push tardisbank/nginx:$TAG

elif [ $# -gt 1 -a "$1" == "tag" ] 
then
    TAG=$2

    echo "Tagging TardisBank ($TAG)"
    echo "-------------------------"

    docker tag tardisbank/server tardisbank/server:$TAG
    docker tag tardisbank/client tardisbank/client:$TAG
    docker tag tardisbank/nginx tardisbank/nginx:$TAG

else
    echo "Building TardisBank"
    echo "-------------------"

    chmod u+x $DIR/server/build.sh
    $DIR/server/build.sh
 
    chmod u+x $DIR/client/build.sh
    $DIR/client/build.sh

    chmod u+x $DIR/nginx/build.sh
    $DIR/nginx/build.sh
fi
