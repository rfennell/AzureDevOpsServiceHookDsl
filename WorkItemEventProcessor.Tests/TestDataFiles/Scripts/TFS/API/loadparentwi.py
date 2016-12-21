wi = GetWorkItem(297)
parentwi = GetParentWorkItem(wi)
if parentwi == None:
    print("Work item '" + str(wi.id) + "' has no parent")
else:
    print("Work item '" + str(wi.id) + "' has a parent '" + str(parentwi.id) + "' with the title '" + str(wi.fields["System.Title"]) +"'")