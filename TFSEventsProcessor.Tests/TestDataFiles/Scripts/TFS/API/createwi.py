# Add  key-value tuples to the dictionary
fields = {"System.Title": "The Title","Microsoft.VSTS.Scheduling.Effort": 2}  

wi = CreateWorkItem("SampleProject","Bug",fields)
print("Work item '" + str(wi.id) + "' has been created with the title '" + str(wi.fields["System.Title"]) +"'")