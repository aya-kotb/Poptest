How To:

1) Test Scene has Path finding PC and one NPC
2) NPC has NPCinterctionstate and NPCActionstate attached to it.
3) To Detect player within specified area or within proximity of npc, use interaction type as proximity or LocationOfPC respectively
4) To manually interact with npc,set interaction state as manual
5) To interact with mouse click on npc, use interaction type as mouseclick
6) Set action type as follow the PC or avoid the target
7) Set manual action in updateaction unityevent and set action type as triggerOnlyOnce

Example
8) Set interaction type as MouseClick
9) Manual action can be used to show dialogs and then with loop true.
10) Whenever you click on npc, you can make it so that it will show different dialog each time.