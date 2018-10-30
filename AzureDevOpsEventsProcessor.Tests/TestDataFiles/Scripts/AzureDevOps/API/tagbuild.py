id = 123
build = GetBuildDetails(id)
AddBuildTag(str(build["url"]), "The Tag")
print("Set tag for build for '" + str(id) + "'")