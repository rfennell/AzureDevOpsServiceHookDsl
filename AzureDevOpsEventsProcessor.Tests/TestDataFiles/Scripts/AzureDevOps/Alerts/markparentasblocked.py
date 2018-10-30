import sys
# Expect 2 args the event type and a value unique ID for the wi
if sys.argv[0] == "workitem.updated" : 
    wi = GetWorkItem(int(sys.argv[1]))
    parentwi = GetParentWorkItem(wi)
    if parentwi == None:
        LogInfoMessage("Work item '" + str(wi.id) + "' has no parent")
    else:
        LogInfoMessage("Work item '" + str(wi.id) + "' has parent '" + str(parentwi.id) + "'")

        # check in case there is no tag set
        tags = ""
        if parentwi["fields"]["System.Tags"] is not None :
            tags = str(parentwi["fields"]["System.Tags"])
        LogInfoMessage("Current tags set '" + tags + "'") 

        results = [c for c in GetChildWorkItems(parentwi) if c["fields"]["Microsoft.VSTS.CMMI.Blocked"] == "Yes"]
        if  len(results) == 0 :
            LogInfoMessage("No child work items are 'BLOCKED'")
            parentwi["fields"]["System.Tags"] = tags.replace("BLOCKED","")
            UpdateWorkItem(parentwi)
            msg = "Work item '" + str(parentwi.id) + "' has been had 'BLOCKED' tag removed as no child work items are BLOCKED"
            LogInfoMessage(msg)
        else:
            LogInfoMessage("Some child work items are 'BLOCKED'")
            parentwi["fields"]["System.Tags"] = tags + ", BLOCKED"
            UpdateWorkItem(parentwi)
            msg = "Work item '" + str(parentwi.id) + "' has had 'BLOCKED' tag added as one or more child work item is BLOCKED"
            LogInfoMessage(msg)
else:
	LogErrorMessage("Was not expecting to get here")
	LogErrorMessage(sys.argv)