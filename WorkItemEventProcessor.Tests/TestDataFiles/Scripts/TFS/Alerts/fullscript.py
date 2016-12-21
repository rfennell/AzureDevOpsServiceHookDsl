import sys
# Expect at least 2 args the event type and a value unique ID
# This is the list of event types currently supported
if sys.argv[0] == "build.complete" :
	print ("A JSON build.complete event " + sys.argv[1])
elif sys.argv[0] == "workitem.created" :
	print ("A JSON workitem.created event " + sys.argv[1])
elif sys.argv[0] == "workitem.updated" :
	print ("A JSON workitem.updated event " + sys.argv[1])
elif sys.argv[0] == "git.push" : 
	print ("A JSON git.push event on repo " + sys.argv[1] + " with id " + sys.argv[2] )
elif sys.argv[0] == "tfvc.checkin" : 
	print ("A JSON tfvc.checkin event " + sys.argv[1])
else:
	print ("Was not expecting to get here")
	print sys.argv

	