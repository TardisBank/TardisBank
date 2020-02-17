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
    docker push tardisbank/db:$TAG

elif [ $# -gt 1 -a "$1" == "tag" ] 
then
    TAG=$2

    echo "Tagging TardisBank ($TAG)"
    echo "-------------------------"

    docker tag tardisbank/server tardisbank/server:$TAG
    docker tag tardisbank/client tardisbank/client:$TAG
    docker tag tardisbank/nginx tardisbank/nginx:$TAG
    docker tag tardisbank/db tardisbank/db:$TAG

elif [ "$1" == "deploy" ] 
then 
    echo "Deploying TardisBank"
    echo "-------------------------"

    scp docker-compose.yml docker-compose.prod.yml root@dev.tardisbank.net:/root/tardisbank 
    ssh root@dev.tardisbank.net "cd tardisbank && ls && docker-compose -f docker-compose.prod.yml -f docker-compose.yml down && docker-compose pull && docker-compose -f docker-compose.prod.yml -f docker-compose.yml up -d"

else
    echo "Building TardisBank"
    echo "-------------------"

    chmod u+x $DIR/server/build.sh
    $DIR/server/build.sh
 
    chmod u+x $DIR/client/build.sh
    $DIR/client/build.sh

    chmod u+x $DIR/nginx/build.sh
    $DIR/nginx/build.sh

    chmod u+x $DIR/db/build.sh
    $DIR/db/build.sh
fi
