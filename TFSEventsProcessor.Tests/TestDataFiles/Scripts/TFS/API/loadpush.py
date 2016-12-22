p = GetPushDetails("3c4e22ee-6148-45a3-913b-454009dac91d",73)
print("Push '" + str(p["pushId"]) + "' contains " + str(len(p["commits"])) + " commits")
for c in p["commits"] :
    detail = GetCommitDetails(str(c["url"]))
    print ("Commit " + str(detail["commitId"]))
