import sys
# Expect 2 args the event type and a value unique ID for the build
if sys.argv[0] == "build.complete" : 
    build = GetBuildDetails(int(sys.argv[1]))
    # Not that sensible a demo as the tag is probably not there 
    if str(build["result"]) == "succeeded" : 
        AddBuildTag(str(build["url"]), "Good Build")
        msg = "'" + str(build["buildNumber"]) + "' tag set'"
    else:
        RemoveBuildTag(str(build["url"]), "Good Build")
        msg = "'" + str(build["buildNumber"]) + "' tag removed'"
    SendEmail("richard@blackmarble.co.uk", str(build["buildNumber"]) + "tag changed " , msg)
    LogInfoMessage(msg)
else:
    LogErrorMessage("Was not expecting to get here")
    LogErrorMessage(sys.argv)
