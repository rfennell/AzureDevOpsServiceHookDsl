import sys
# Expect 2 args the event type and a value unique ID for the build
if sys.argv[0] == "build.complete" : 
	build = GetBuildDetails(int(sys.argv[1]))
	if str(build["result"]) == "succeeded" : 
		majorVersion = GetBuildArgument(str(build["definition"]["url"]),"MajorVersion")
		newMinorVersion = IncrementBuildArgument(str(build["definition"]["url"]),"MinorVersion")
		msg = "'" + str(build["definition"]["name"]) + "' version incremented to " + majorVersion + "." + str(newMinorVersion) +".x.x"
		SendEmail("richard@blackmarble.co.uk",str(build["definition"]["name"]) + " version incremented", msg)
		LogInfoMessage(msg)
else:
	LogErrorMessage("Was not expecting to get here")
	LogErrorMessage(sys.argv)
