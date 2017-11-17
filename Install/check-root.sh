#!/bin/bash

#check for root
if [ "$UID" -eq 0 ]
then
    echo "youre root! thats good!"
else
    echo "you have to be root do execute this"
    exec sudo -H bash "$0" "$@"; exit 0
fi