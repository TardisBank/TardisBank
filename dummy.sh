#!/usr/bin/env bash
set -e

PUSH_DOCKER="false"
APPVEYOR_REPO_TAG="true"
APPVEYOR_RE_BUILD="true"
APPVEYOR_REPO_TAG_NAME="v1.0.18"
if [ "${APPVEYOR_REPO_TAG}" == "true" -a "${APPVEYOR_RE_BUILD}" == "true" ]
then
    PROD_DEPLOY="true"
    TAG_NAME="${APPVEYOR_REPO_TAG_NAME:1}"
    echo "This is a production deployment for ${env:TAG_NAME}."
fi

if [[ ! -z "${DOCKER_USER}" ]] && [[ ! -z "${DOCKER_PASSWORD}" ]]
then 
    PUSH_DOCKER="true"
fi

if [ "${APPVEYOR_REPO_TAG}" == "true" ]
then
    tagName="${APPVEYOR_REPO_TAG_NAME:1}"
    RELEASE_NAME="Version ${tagName}"
fi

echo "push ${PUSH_DOCKER}"
echo "prod deploy ${PROD_DEPLOY}"
echo "tag name ${TAG_NAME}"
echo "release name ${RELEASE_NAME}"