cs = GetChangesetDetails(7)
print("Changeset '" + str(cs["changesetId"]) + "' has the comment '" + str(cs["comment"]) +"' and contains " + str(len(cs["changes"])) + " files")
