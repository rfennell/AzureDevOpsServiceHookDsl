wi = GetWorkItem(23)
for childwi in GetChildWorkItems(wi):
    print("Work item '" + str(wi.id) + "' has a child '" + str(childwi.id) + "' with the title '" + str(childwi.fields["System.Title"]) +"'")