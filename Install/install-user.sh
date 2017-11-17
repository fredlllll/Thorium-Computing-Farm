#!/bin/bash

salt=$(date +%s)
encpass=$(perl -e 'print crypt($ARGV[0], $ARGV[1])' $PASSWORD $salt)
useradd -m -p $encpass -s /bin/bash $USER