#!/usr/bin/env bash

function handle_error()
{
    _RET=${PIPESTATUS[0]}
    if [[ $_RET != 0 ]]; then
        echo "*** Error: The previous step failed!"

        cd ../../
        exit $_RET
    fi
}

echo "Starting to build the frontend for ASP.NET MVC 4 sample ..."
echo ""

echo "Installing Node.js packages ..."
echo ""
npm install
handle_error
echo ""

echo "Installing Bower packages ..."
echo ""
bower install
handle_error
echo ""

echo "Building client-side assets ..."
echo ""
gulp
handle_error
echo ""

echo "Succeeded!"

cd ../../
exit $_RET