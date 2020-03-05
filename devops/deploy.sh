#!/bin/bash

set -e
#
# Run the given remote <cmd>.
#

run() {
  ssh $REMOTE_USER@$REMOTE_HOST $@
}

DEPLOY_FOLDER="~/deployment/${COMPOSE_PROJECT_NAME}"

run "mkdir -p $DEPLOY_FOLDER" 
scp docker-compose.yml docker-compose.prod.yml ./devops/.env $REMOTE_USER@$REMOTE_HOST:$DEPLOY_FOLDER
run "cd $DEPLOY_FOLDER && docker-compose -f docker-compose.prod.yml -f docker-compose.yml down"
run "cd $DEPLOY_FOLDER && docker-compose -f docker-compose.prod.yml -f docker-compose.yml pull"
run "cd $DEPLOY_FOLDER && docker-compose -f docker-compose.prod.yml -f docker-compose.yml up -d"
