using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Poptropica2.CharacterDialogUI {
    /// <summary>
    /// The DialogUI Canvas component
    /// Inherits from AbstractDialogueUI inside the "Dialogue System For Unity" plugin
    /// Creates NPC dialogs at runtime - More than one maybe required - Based on the functionality in Pop 1.0
    /// Creates barks at runtime - For mic announcements .etc. Bark System is a feature inside the "Dialogue System For Unity" plugin which suits this need
    /// 
    /// The plugin has it's own system for doing things. This was modified to suit our needs. 
    /// However there's a Dialogue Manager(Necessity from the plugin's end) which is a singleton.
    /// 
    /// The plugin had to have one Subtitle Panel pre-assigned to any character/NPC/Barkers. 
    /// In the modified system the player has it's pre-assigned Subtitle Panel and Response Panel. 
    /// However, the barks UI and the NPC Subtitle Panels are generated based on need during runtime.
    /// 
    /// The DialogUICanvas is under this Dialogue Manager within the Hierarchy. 
    /// The DialogUICanvas is responsible for creating UI of player dialogues/responses , NPC dialogs and background barks. 
    /// </summary>
    public class DialogUICanvas : AbstractDialogueUI {

        public Transform npcSubtitlePanel;
        public Transform responsePanel;
        public Transform playerDialogPanel;

        public GameObject dialogPrefab;
        public Transform barkContainer;

        public float speechBubbleYOffsetPC = 1f;                   //The speech bubbles positions are dependent on the player's character size. This variable is just an offset added to that.
        public float speechBubbleYOffsetNPC = 1f;                  //The speech bubbles positions are dependent on the NPC's character size. This variable is just an offset added to that.

        List<DialogUI> dialogUIs;
      
        /// <summary>
        /// The UI root.
        /// </summary>
        [HideInInspector] public UnityUIRoot unityUIRoot;

        /// <summary>
        /// The dialogue controls used in conversations.
        /// </summary>
        public CustomUIDialogControls dialogue;

        /// <summary>
        /// QTE (Quick Time Event) indicators.
        /// </summary>
        public UnityEngine.UI.Graphic[] qteIndicators;

        /// <summary>
        /// The alert message controls.
        /// </summary>
        public UnityUIAlertControls alert;

        /// <summary>
        /// Set <c>true</c> to always keep a control focused; useful for gamepads.
        /// </summary>
        [Tooltip("Always keep a control focused; useful for gamepads")]
        public bool autoFocus = false;

        /// <summary>
        /// Set <c>true</c> to look for OverrideUnityUIDialogueControls on actors.
        /// </summary>
        [Tooltip("Look for OverrideUnityUIDialogueControls on actors")]
        public bool findActorOverrides = false;

        /// <summary>
        /// Set <c>true</c> to add an EventSystem if one isn't in the scene.
        /// </summary>
        [Tooltip("Add an EventSystem if one isn't in the scene")]
        public bool addEventSystemIfNeeded = true;

        private UnityUIQTEControls qteControls;

        public override AbstractUIRoot UIRoot {
            get { return unityUIRoot; }
        }

        public override AbstractDialogueUIControls Dialogue {
            get { return dialogue; }
        }

        public override AbstractUIQTEControls QTEs {
            get { return qteControls; }
        }

        public override AbstractUIAlertControls Alert {
            get { return alert; }
        }

        // References to the original controls in case an actor temporarily overrides them:
        private UnityUISubtitleControls originalNPCSubtitle;
        private UnityUISubtitleControls originalPCSubtitle;
        private UnityUIResponseMenuControls originalResponseMenu;

        // Caches overrides by actor so we only need to search an actor once:
        private Dictionary<Transform, OverrideUnityUIDialogueControls> overrideCache = new Dictionary<Transform, OverrideUnityUIDialogueControls>();

        #region Initialization

        /// <summary>
        /// Sets up the component.
        /// </summary>
        public override void Awake() {
            base.Awake();
            FindControls();
            dialogUIs = new List<DialogUI>();
        }

        /// <summary>
        /// Logs warnings if any critical controls are unassigned.
        /// </summary>
        private void FindControls() {
            UITools.RequireEventSystem();
            qteControls = new UnityUIQTEControls(qteIndicators);
            if (DialogueDebug.LogErrors) {
                if (DialogueDebug.LogWarnings) 
                {
                    if (dialogue.pcSubtitle.line == null) Debug.LogWarning(string.Format("{0}: UnityUIDialogueUI PC Subtitle Line needs to be assigned.", DialogueDebug.Prefix));
                    if (dialogue.responseMenu.buttons.Length == 0 && dialogue.responseMenu.buttonTemplate == null) Debug.LogWarning(string.Format("{0}: UnityUIDialogueUI Response buttons need to be assigned.", DialogueDebug.Prefix));
                }
            }
            originalNPCSubtitle = dialogue.npcSubtitle;
            originalPCSubtitle = dialogue.pcSubtitle;
            originalResponseMenu = dialogue.responseMenu;
        }

        private OverrideUnityUIDialogueControls FindActorOverride(Transform actor) {
            if (actor == null) return null;
            if (!overrideCache.ContainsKey(actor)) {
                overrideCache.Add(actor, (actor != null) ? actor.GetComponentInChildren<OverrideUnityUIDialogueControls>() : null);
            }
            return overrideCache[actor];
        }

        public void OnLevelWasLoaded(int level) {
            UITools.RequireEventSystem();
        }

        public override void Open()
        {
            overrideCache.Clear();
            base.Open();
        }

        #endregion

        #region Subtitles
        /// <summary>
        /// Function inside the IDialogueUI interface from the plugin
        /// Modified to create more than one npc dialogs
        /// Hides player responses if new subtitle appears
        /// </summary>
        /// <param name="subtitle">Subtitle.</param>
        public override void ShowSubtitle(Subtitle subtitle) {
            HideResponses();
            if (subtitle.speakerInfo == null || subtitle.speakerInfo.characterType == CharacterType.NPC) 
            {
                float duration = DialogueManager.DisplaySettings.subtitleSettings.minSubtitleSeconds;
                Collider2D nPCBodyCollider = subtitle.speakerInfo.transform.GetComponent<Usable>().bodyCollider;
                Vector2 dialogPosition = (Vector2)nPCBodyCollider.bounds.center + new Vector2(0,(nPCBodyCollider.bounds.size.y / 2));
                dialogue.npcSubtitle = CreateNPCSubTitle(dialogPosition, subtitle, duration);
            }
            base.ShowSubtitle(subtitle);
            CheckSubtitleAutoFocus(subtitle);
        }

        /// <summary>
        /// Function inside the IDialogueUI interface from the plugin
        /// Modified to nullify the npcSubtitle so that more than one npc can have their dialogs on screen 
        /// </summary>
        /// <param name="subtitle">Subtitle.</param>
        public override void HideSubtitle(Subtitle subtitle) {
            dialogue.npcSubtitle = null;
            base.HideSubtitle(subtitle);
        }

        public void CheckSubtitleAutoFocus(Subtitle subtitle) {
            if (autoFocus) {
                if (subtitle.speakerInfo.IsPlayer) {
                    dialogue.pcSubtitle.AutoFocus();
                } else {
                    dialogue.npcSubtitle.AutoFocus();
                }
            }
        }


        public void OnConversationEnd()
        {
            dialogue.pcSubtitle.Hide();
            HideResponses();
        }

        public bool IsResponseMenuActive()
        {
            if(responsePanel.gameObject.activeInHierarchy)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Responses
        /// <summary>
        /// Shows the responses of the player
        /// Function inside the IDialogueUI interface from the plugin
        /// </summary>
        /// <param name="subtitle">Subtitle.</param>
        /// <param name="responses">Responses.</param>
        /// <param name="timeout">Timeout.</param>
        public override void ShowResponses (Subtitle subtitle, Response[] responses, float timeout) {

            base.ShowResponses(subtitle, responses, timeout);
            CheckResponseMenuAutoFocus();
        }

        public void CheckResponseMenuAutoFocus() {
            if (autoFocus) dialogue.responseMenu.AutoFocus();
        }

        public override void HideResponses() {
            dialogue.responseMenu.DestroyInstantiatedButtons();
            base.HideResponses();
        }

        #endregion



        #region PopTropica functionality

        /// <summary>
        /// Creates NPC subtitle at runtime
        /// The plugin originally used just one instance of text for npc conversations
        /// This method enables more than one instance of npc dialog bubbles - as observed in Pop 1.0
        /// However, the player will respond to the last NPC interacted with (The player has only one instance of dialog text and responses)
        /// The Subtitle component is a feature from the plugin. It contains the conversation text, speakerinfo.etc.
        /// This method also converts the world position of the NPC to screen position
        /// 
        /// 
        /// Since NPC dialogs are created at runtime, the pivots might require changing too based on where the dialogs are shown
        /// </summary>
        /// <returns>The NPC sub title.</returns>
        /// <param name="position">World position of the subtitle panel</param>
        /// <param name="subtitle">The Subtitle component is a feature from the plugin. It contains the conversation text, speakerinfo.etc.</param>
        /// <param name="duration">How long the subtitle will stay on screen before disappearing</param>
        public UnityUISubtitleControls CreateNPCSubTitle(Vector2 position,Subtitle subtitle, float duration)
        {
            Vector2 pivot = new Vector2(0.5f, 0f);                                      //This value might rarely be required for changing. It is centered on X and Y pivot is at the bottom of the dialog UI
                                                                                        //Based on Pop 1.0 , all the npc dialogs popup above the character

            Vector3 positionWithYOffset = position + new Vector2(0, speechBubbleYOffsetNPC);

            DialogUI dialogUI =  CreateDialog(npcSubtitlePanel, positionWithYOffset, pivot, duration, subtitle);

            UnityUISubtitleControls npcSubtitle = new UnityUISubtitleControls();

            npcSubtitle.line = dialogUI.dialogText;
            npcSubtitle.panel = dialogUI.GetComponent<Graphic>();

            return npcSubtitle;
        }

        /// <summary>
        /// Creates a dialog from the dialogPrefab
        /// </summary>
        /// <returns>The dialog.</returns>
        /// <param name="parent">The parent gameobject of the created dialog</param>
        /// <param name="position">Position in world space</param>
        /// <param name="pivot">The pivot can be adjusted based on positioning needs</param>
        /// <param name="duration">How long the subtitle will stay on screen before disappearing</param>
        /// <param name="subtitle">The Subtitle component is a feature from the plugin. It contains the conversation text, speakerinfo.etc.</param>
        DialogUI CreateDialog(Transform parent, Vector2 position, Vector2 pivot, float duration, Subtitle subtitle)
        {
            DialogUI dialogUI = CheckForSpeakerDialogUI(subtitle);
           
            if(dialogUI!=null)
            {
                dialogUI.Duration = duration;
                SetDialogUIText(dialogUI, subtitle);
                return dialogUI;
            }

            GameObject dialog = Instantiate(dialogPrefab);
            dialog.transform.SetParent(parent);
            dialog.transform.localScale = Vector3.one;


            dialogUI = dialog.GetComponent<DialogUI>();
            dialogUI.SpeakerTransform = subtitle.speakerInfo.transform;
            dialogUI.Duration = duration;
            dialogUIs.Add(dialogUI);

            //Vector3 targetPosition = Camera.main.WorldToScreenPoint(position);
            dialog.transform.position = position;

            dialogUI.SetPivot(pivot);

            SetDialogUIText(dialogUI, subtitle);


           

            StartCoroutine(DestroyDialogUI(dialogUI));

            return dialogUI;
        }

        /// <summary>
        /// Sets the text fot the DialogUI component
        /// </summary>
        /// <param name="dialogUI">A DialogUI component</param>
        /// <param name="subtitle">Subtitle.</param>
        void SetDialogUIText(DialogUI dialogUI, Subtitle subtitle)
        {
            string subtitleText = subtitle.formattedText.text;
            if (dialogUI.dialogText != null) 
            {
                dialogUI.dialogText.text = subtitleText;
            }
        }

        /// <summary>
        /// Creates a bark
        /// Can be used for mic announcements .etc. Bark System is a feature inside the "Dialogue System For Unity" plugin which suits this need
        /// The pivot can be adjusted based on positioning needs
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="subtitle">Subtitle.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="pivot">Pivot of the created dialog</param>
        public void CreateBark(Vector2 position,Subtitle subtitle, float duration, Vector2 pivot)
        {
            CreateDialog(barkContainer, position, pivot, duration, subtitle);
        }

        /// <summary>
        /// Updates the player dialog position to the current player position
        /// Has an offset to be placed above the player
        /// This method also converts the world position of the player to screen position
        /// </summary>
        /// <param name="playerPosition">Position of the player in world coordinates</param>
        public void UpdatePlayerDialogPosition(Vector2 playerPosition)
        {
            Vector3 playerPositionWithYOffset = playerPosition + new Vector2(0, speechBubbleYOffsetPC);
            //Vector3 targetPosition = Camera.main.WorldToScreenPoint(playerPositionWithYOffset);
            playerDialogPanel.position = playerPositionWithYOffset;
            responsePanel.position = playerPositionWithYOffset;
        }

        /// <summary>
        /// Checks if there's a DialogUI component for the speaker within the dialogUIs list
        /// </summary>
        /// <returns>DialogUI component if there's one already in the scene for the speaker of the subtitle</returns>
        /// <param name="subtitle">Subtitle.</param>
        DialogUI CheckForSpeakerDialogUI(Subtitle subtitle)
        {
            for(int i=0; i<dialogUIs.Count; i++)
            {
                if(dialogUIs[i].SpeakerTransform == subtitle.speakerInfo.transform)
                {
                    return dialogUIs[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Destroys the dialogUI gameobject once it's duration expires
        /// Also removes it from the dialogUIs list
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="dialogUI">A dialogUI component.</param>
        IEnumerator DestroyDialogUI(DialogUI dialogUI)
        {
            while(dialogUI.Duration>0)
            {
                dialogUI.Duration -= Time.deltaTime;
                yield return null;
            }
            dialogUIs.Remove(dialogUI);
            Destroy(dialogUI.gameObject);
           
        }
            
        #endregion
}
}
