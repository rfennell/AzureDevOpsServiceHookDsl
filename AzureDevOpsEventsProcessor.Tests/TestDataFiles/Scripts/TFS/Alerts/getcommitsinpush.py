import sys
# Expect 3 args the event type and a value unique ID
if sys.argv[0] == "git.push" : 
    print ("A JSON git.push event for repo " + sys.argv[1] + " and ID " +  sys.argv[2])
    p = GetPushDetails(sys.argv[1], int(sys.argv[2]))
    msg = "Pull '" + str(p["pushId"]) + "' contains " + str(len(p["commits"])) + " commit(s)"
    for c in p["commits"] :
        msg += "<li>  Commit:" + str(c["commitId"])
    SendEmail("richard@blackmarble.co.uk", "Push: " + str(p["pushId"]) , msg)
    LogInfoMessage(msg)
else:
	LogErrorMessage("Was not expecting to get here")
	LogErrorMessage(sys.argv)

	