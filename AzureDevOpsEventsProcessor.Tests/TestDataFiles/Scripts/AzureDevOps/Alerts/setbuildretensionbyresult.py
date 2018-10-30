import sys
# Expect 2 args the event type and a value unique ID for the build
if sys.argv[0] == "build.complete" : 
	build = GetBuildDetails(int(sys.argv[1]))
	if str(build["result"]) != "succeeded" : 
		keepForever = False
	else:
	    keepForever = True
	# Will get a 403 error if already set
	SetBuildRetension(str(build["url"]), keepForever) 
	msg = "'" + str(build["buildNumber"]) + "' retension set to '" + str(keepForever) + "' as result was '" + str(build["result"]) + "'"
	SendEmail("richard@blackmarble.co.uk", str(build["buildNumber"]) + " retension set to " + str(keepForever)  , msg)
	LogInfoMessage(msg)
else:
	LogErrorMessage("Was not expecting to get here")
	LogErrorMessage(sys.argv)
