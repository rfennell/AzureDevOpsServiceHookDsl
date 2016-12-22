# Do something using one loaded library, the sample 
v1 = 1
v2 = 2
sum = Add(v1,v2)
msg = "When you add " + str(v1) + " and " + str(v2) + " you get " + str(sum)
# then use the primary DSL to log results
LogInfoMessage(msg)
# and check the providers are also passed to the secondary libray
SampleSendEmail("fred@test.com","The subject",msg)
