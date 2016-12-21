![Build Status - VSTSServiceHookDsl]https://richardfennell.visualstudio.com/DefaultCollection/_apis/public/build/definitions/670b3a60-2021-47ab-a88b-d76ebd888a2f/13/badge)

This project replace the Codeplex Repo [https://tfsalertsdsl.codeplex.com](https://tfsalertsdsl.codeplex.com), which supported the older SOAP based alerts model. If you need to use this older model use this Codeplex project. 

# TFS & VSTS Alerts DSL 

Since it's inception Microsoft Team Foundation Server (TFS) provides a SOAP based alerting model where given a certain condition, such as a check-in, work item edit or build completion, an email can be sent to an interest party or a call made to a SOAP based web service. Using this SOAP model it is possible to provide any bespoke operations you wish that are triggered by a change on the TFS server.

Recent versions of TFS and the hosted Visual Studio Teams Services (VSTS) also offer a REST Web Hook based alerts. This is now the preferred method for handling alerts. 

This framework is designed to ease the development of a REST webhook services by providing helper methods for common processing steps and API operations such as calling back to the TFS server or accessing SMTP services.

The key feature of this project is that it provides a Python based DSL that allows the actual operation performed when the endpoint is called to be edited without the need to  to rebuild and redeploy the bespoke service. Operations are defined by script such as show below

```
import sys
# Expect 2 args the event type and a value unique ID
LogInfoMessage( "The following arguments were passed" )
LogInfoMessage( sys.argv )
if sys.argv[0] == "build.complete" :
   LogInfoMessage ("A build event " + sys.argv[1])
   # a sample for using the DSL to create a work item is
   #fields = {"Title": "The Title","Effort": 2, "Description": "The desc of the bug"}
   #teamproject = "Scrum (TFVC)"
   #wi = CreateWorkItem(teamproject ,"bug",fields)
   #LogInfoMessage ("Work item '" + str(wi.id) + "' has been created '" + str(wi.fields["System.Title"]) +"'")
elif sys.argv[0] == "git.push" : 
	print ("A JSON git.push event " + sys.argv[1])
elif sys.argv[0] == "tfvc.checkin" : 
	print ("A JSON tfvc.checkin event " + sys.argv[1])
else:
	print ("Was not expecting to get here")
	print sys.argv

 ```
