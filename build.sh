#! /bin/bash

cd ./server
docker build . --target build
cd ..

cd ./client
docker build .
cd ..
