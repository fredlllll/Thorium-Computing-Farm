USER="thorium"

su $USER <<EOM
git fetch
git pull
git submodule update --init --recursive
git submodule update --recursive --remote
cd Source
nuget restore
msbuild
cd ..
EOM