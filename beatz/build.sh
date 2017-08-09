#!/usr/bin/env bash

set -e

docker pull microsoft/dotnet:latest

docker build -t beatz -f beatz/Dockerfile .

docker images