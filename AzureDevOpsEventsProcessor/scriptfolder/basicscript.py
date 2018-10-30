import sys
# Expect at least 2 args the event type and a value unique ID
if sys.argv[0] == "git.push" : 
	msg = "A JSON " + sys.argv[0] + " event on repo " + sys.argv[1] + " with id " + sys.argv[2] 
else:
	msg = "A JSON " + sys.argv[0] + " event with id " + sys.argv[1] 
LogInfoMessage(msg)	