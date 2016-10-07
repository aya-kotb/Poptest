How To:
1) I have assigned one dummy quest in listOfQuests in QuestTester
2) QuestTester has Listen method where steps can listen to for specific events
3) Whenever button is pressed, i am calling the associated step related to that button
4) Button names are same as object id for step condition types for testing
5) Pick hammer is not mandatory step
6) Each quest data is inside statemachine and stage data within queststage
7) For event system, currently using QuestTester
8) Whenever step is completed, callback on queststage will be called
9) Whenever stage is completed, callback on quest is called.
10) when all mandatory steps are completed, stage will be completed as well
11) when all stages are completed, quest will be completed.
12) Only has one quest, will work with more quests