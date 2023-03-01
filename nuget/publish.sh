R='\033[0;31m'   #'0;31' is Red's ANSI color code
G='\033[0;32m'   #'0;32' is Green's ANSI color code
Y='\033[0;33m'   #'1;32' is Yellow's ANSI color code
B='\033[0;34m'   #'0;34' is Blue's ANSI color code
D='\033[0m'      #'0' is the ANSI color code for Default
NUGET_URL="https://api.nuget.org/v3"
PACKAGE_NAME="KalorAform.MultiwayTree"

echo -e "\n${B}Parsing package version.${D}"
VERSION_LINE=$(grep Version "../$PACKAGE_NAME/$PACKAGE_NAME.csproj")
VERSION=$(echo $VERSION_LINE| cut -d'>' -f 2| cut -d'<' -f 1)
echo -e "${B}Found package version: ${D}$VERSION"

echo -e "${B}Checking nuget to see if this is a new package version.${D}"
REG_SERVER_URL="$NUGET_URL/registration5-semver1/$PACKAGE_NAME/$VERSION"
REG_CURL_RESULT=$(curl -s $REG_SERVER_URL)

if [[ $REG_CURL_RESULT == *"BlobNotFound"* ]]; then
	echo -e "${B}Current package version does not exist. Publishing to nuget.org.${D}\n"
	NUGET_PUSH_RESULT=$(dotnet nuget push "../$PACKAGE_NAME/bin/Debug/$PACKAGE_NAME.$VERSION.nupkg" --api-key $1 --source https://api.nuget.org/v3/index.json)
	
	if [[ $NUGET_PUSH_RESULT == *"409"* ]]; then
		echo -e "${Y}The registration server lied to us. This version of the package already exists. If this is not expected, increment the version number.${D}"
	elif [[ $NUGET_PUSH_RESULT == *"Your package was pushed"* ]]; then
		echo -e "${G}Successfully published new package version.${D}"
	elif [[ $NUGET_PUSH_RESULT == *"File does not exist"* ]]; then
		echo -e "${R}$NUGET_PUSH_RESULT"
	else
		echo -e "\n${R}The nuget server returned an unexpected response: $NUGET_PUSH_RESULT"
	fi
fi