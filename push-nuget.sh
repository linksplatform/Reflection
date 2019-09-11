#!/bin/bash
set -e # Exit with nonzero exit code if anything fails

dotnet pack -c Release
dotnet nuget push -s https://api.nuget.org/v3/index.json -k ${NUGETTOKEN} **/*.nupkg
find . -type f -name '*.nupkg' -delete
find . -type f -name '*.snupkg' -delete
