import sys
# Expect 2 args the event type and a value unique ID
LogInfoMessage( "The following arguments were passed" )
LogInfoMessage( sys.argv )
if sys.argv[0] == "BuildEvent" : 
   LogInfoMessage ("A build event " + sys.argv[1])
   # a sample for using the DSL to create a work item is
   #fields = {"Title": "The Title","Effort": 2, "Description": "The description of the bug"}
   #wi = CreateWorkItem("Scrum (TFVC)","bug",fields)
   #LogInfoMessage ("Work item '" + str(wi.Id) + "' has been created with the title '" + wi.Title +"'")
elif sys.argv[0] == "WorkItemEvent" :
	LogInfoMessage ("A wi event " + sys.argv[1])
elif sys.argv[0] == "CheckInEvent" :
	LogInfoMessage ("A checkin event " + sys.argv[1])
else:
	LogInfoMessage ("Was not expecting to get here")

	