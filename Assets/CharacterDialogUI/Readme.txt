1. We used the "Dialogue System For Unity" plugin for this module
2. The test scene is inside the CharacterDialogUI Folder. 
3. The test scene contains 2 NPCs (In the shape of circles)
4. It also contains a background microphone (In the shape of a cross)

5. A temporary dialog database was created using the plugin for the conversations and barks. (Bark system is a feature in the plugin for elements which can spawn dialogs without any interaction)

6. Now, the plugin has it's own system for doing things. This was modified to suit our needs. However there's a Dialogue Manager(Necessity from the plugin's end) within the scene which is a singleton. If you have any suggestions to avoid it, please let us know.

7. The DialogUICanvas is under this Dialogue Manager within the Hierarchy. The DialogUICanvas is responsible for creating UI of player dialogues/responses , NPC dialogs and background barks. 

8. The plugins way of doing things was a bit different than what we needed. The plugin had to have one Subtitle Panel pre-assigned to any character/NPC/Barkers. The player has it's pre-assigned Subtitle Panel and Response Panel. However, the barks UI and the NPC Subtitle Panels are generated based on need during runtime.

9. There's one CharacterActions component attached to the Character. This is interacting with the NPCs to start conversations. 

10. Any NPC capable of conversating should have a Conversation Trigger component attached to them along with a Usable component. (These components belong to the plugin).

11. Any NPC capable of barking should have a BarkOnIdle component attached to them and a BarkToUI component.