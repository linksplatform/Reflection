#!/bin/bash
set -e # Exit with nonzero exit code if anything fails

# Pack NuGet package
dotnet pack -c Release

# Get version string
PackageFileNamePrefix="Platform.${TRAVIS_REPO_NAME}/bin/Release/Platform.${TRAVIS_REPO_NAME}."
PackageFileNameSuffix=".nupkg"
PackageFileName=$(echo ${PackageFileNamePrefix}*${PackageFileNameSuffix})
Version="${PackageFileName#$PackageFileNamePrefix}"
Version="${Version%$PackageFileNameSuffix}"

# Ensure NuGet package does not exist
NuGetPackageUrl="https://globalcdn.nuget.org/packages/Platform.${TRAVIS_REPO_NAME}.${Version}${PackageFileNameSuffix}"
NuGetPackageUrl=$(echo "$NuGetPackageUrl" | tr '[:upper:]' '[:lower:]')
echo $NuGetPackageUrl
NuGetPageStatus="$(curl -Is ${NuGetPackageUrl} | head -1)"
StatusContents=( $NuGetPageStatus )
echo ${StatusContents[1]}
if [ ${StatusContents[1]} == "200" ]; then
  echo "NuGet with current version is already pushed."
  exit 0
fi

# Push NuGet package
dotnet nuget push -s https://api.nuget.org/v3/index.json -k ${NUGETTOKEN} ./**/*.nupkg

# Clean up
find . -type f -name '*.nupkg' -delete
find . -type f -name '*.snupkg' -delete
