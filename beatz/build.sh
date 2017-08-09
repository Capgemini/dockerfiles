#!/usr/bin/env bash

docker pull microsoft/dotnet:latest

docker build -t beatz -f Dockerfile .

docker images