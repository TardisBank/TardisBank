#!/usr/bin/env bash
set -e

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd)"

echo -e "\n##Build db"

echo -e "\nBuilding docker image"
docker --version
docker build -t tardisbank/db $DIR/.