#!/bin/bash
set -ev

pwd
ls -la

echo TRAVIS_PULL_REQUEST == ${TRAVIS_PULL_REQUEST}
echo BUILD_CONFIG == ${BUILD_CONFIG}

echo BUILD_DIR == ${BUILD_DIR}

dotnet build -c $BUILD_CONFIG .
