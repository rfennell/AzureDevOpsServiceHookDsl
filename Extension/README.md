
# Azure DevOps Service & Server Alerts DSL 

Both Azure DevOps Server (TFS) and Azure DevOps Service (VSTS) allow you to integrate with other systems using [Service Hooks](https://www.visualstudio.com/en-us/docs/integrate/get-started/service-hooks/get-started). As well as using the out the box offering it is possible to implement your own integrations using a REST Web Hook endpoint.  

This extension framework is designed to ease the development of your own REST Web Hook web site to do this type of integration. It does this by providing a MVC WebAPI endpoint and a collection of helper methods, implemented as an extensible Domain Specific Language (DSL), for common processing steps and API operations such as calling back to the TFS/VSTS server that called the endpoint or accessing SMTP services.

The key feature of this system is that the actual actions performed when a trigger occurs are controlled by a Python based script(s) that that allows the actual operation performed when the endpoint is called to be edited without the need to  to rebuild and redeploy the bespoke service.

Operations are defined by script such as show below

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
See the [wiki](https://github.com/rfennell/VSTSServiceHookDsl/wiki) for more details