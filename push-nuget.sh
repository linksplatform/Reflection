#!/bin/bash
set -e # Exit with nonzero exit code if anything fails

#status="$(curl -Is https://www.nuget.org/packages/Platform.${TRAVIS_REPO_NAME}/0.1.0 | head -1)"
#validate=( $status )
#if [ ${validate[-2]} == "200" ]; then
#  echo "NuGet with current version is already pushed."
#  exit 0
#fi

dotnet pack -c Release

# Get version string
PackageFileName=$(echo Platform.${TRAVIS_REPO_NAME}/bin/Release/Platform.${TRAVIS_REPO_NAME}.*.nupkg)
PackageFileNamePrefix="Platform.${TRAVIS_REPO_NAME}/bin/Release/Platform.${TRAVIS_REPO_NAME}."
PackageFileNameSuffix=".nupkg"
echo $PackageFileName
Version = "${$PackageFileName#$PackageFileNamePrefix}"
Version = "${$Version%$PackageFileNameSuffix}"
echo $Version

dotnet nuget push -s https://api.nuget.org/v3/index.json -k ${NUGETTOKEN} **/*.nupkg

# Clean up
find . -type f -name '*.nupkg' -delete
find . -type f -name '*.snupkg' -delete
