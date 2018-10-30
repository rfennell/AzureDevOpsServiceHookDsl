import sys
# Expect 2 args the event type and a value unique ID
if sys.argv[0] == "tfvc.checkin" : 
    print ("A JSON tfvc.checkin event " + sys.argv[1])
    cs = GetChangesetDetails(int(sys.argv[1]))
    msg = "Changeset '" + str(cs["changesetId"]) + "' has the comment '" + str(cs["comment"]) +"' and contains " + str(len(cs["changes"])) + " files"
    SendEmail("richard@blackmarble.co.uk", "Changeset '" + str(cs["changesetId"])  , msg)
    LogInfoMessage(msg)
else:
	LogErrorMessage("Was not expecting to get here")
	LogErrorMessage(sys.argv)

	