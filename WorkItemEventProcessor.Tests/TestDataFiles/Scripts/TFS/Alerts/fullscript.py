import sys
# Expect at least 2 args the event type and a value unique ID
# This is the list of event types currently supported
if sys.argv[0] == "git.push" : 
	print ("Got a known " + sys.argv[0] + " event type on repo " + sys.argv[1] + " with id " + sys.argv[2] )
else : 
	print ("Got a known " + sys.argv[0] + " event type with id " + sys.argv[1] )

	