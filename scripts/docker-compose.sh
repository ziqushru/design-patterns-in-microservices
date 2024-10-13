#!/bin/bash

source .env

docker_compose_env=../containers-conf/docker-compose.${EXEC_ENV}.env
docker_compose_yml=../containers-conf/docker-compose.${EXEC_ENV}.yml

if [ $# -eq 0 ]
then
    echo "No arguments supplied"
elif [ $1 = 'build' ]
then
    docker compose --env-file ${docker_compose_env} -f ${docker_compose_yml} build
elif [ $1 = 'up' ]
then
    docker compose --env-file ${docker_compose_env} -f ${docker_compose_yml} up -d --remove-orphans
elif [ $1 = 'down' ]
then
    docker compose --env-file ${docker_compose_env} -f ${docker_compose_yml} down
elif [ $1 = 'log' ]
then
    docker compose --env-file ${docker_compose_env} -f ${docker_compose_yml} logs
fi
