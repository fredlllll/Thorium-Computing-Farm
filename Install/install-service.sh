#!/bin/bash

cp $SERVICENAME.service /etc/systemd/system/$SERVICENAME.service
chmod 644 /etc/systemd/system/$SERVICENAME.service
systemctl enable $SERVICENAME

echo "you can now start the service using systemctl start $SERVICENAME"