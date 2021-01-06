#!/bin/bash
set -ev

TAG=$1
DOCKER_USERNAME=$2
DOCKER_PASSWORD=$3

# Create publish artifact
dotnet publish -c Release -o app/publish

# Build the Docker images
docker build -t burja8x/report:$TAG .
docker tag burja8x/report:$TAG burja8x/report:latest

# Login to Docker Hub and upload images
docker login -u="$DOCKER_USERNAME" -p="$DOCKER_PASSWORD"
docker push burja8x/report:$TAG
docker push burja8x/report:latest