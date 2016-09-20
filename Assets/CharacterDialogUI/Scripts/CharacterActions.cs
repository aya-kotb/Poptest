using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using PixelCrushers.DialogueSystem;
using Poptropica2.Characters;

namespace Poptropica2.CharacterDialogUI {
    /// <summary>
    /// This script is currently roughly based on the Selector script from the "Dialogue System For Unity" plugin
    /// It is used for interacting with other NPCs to start conversations
    /// 
    /// You can hook into SelectedUsableObject and DeselectedUsableObject to get notifications
    /// when the current target has changed.
    /// </summary>
    public class CharacterActions : MonoBehaviour {


        public event SelectedUsableObjectDelegate SelectedUsableObject = null;
        public event DeselectedUsableObjectDelegate DeselectedUsableObject = null;

        [Tooltip("Tick to check all objects within raycast range for usables, even passing through obstacles")]
        public bool raycastAll = false;

        [Tooltip("Don't target objects farther than this; targets may still be unusable if beyond their usable range")]
        public float maxSelectionDistance = 30f;

        [Tooltip("Tick to also broadcast to the usable object's children")]
        public bool broadcastToChildren = true;

        public Collider2D bodyCollider;

        public LayerMask layerMask;

        public DialogUICanvas dialogUICanvas;

        Usable usable = null;
        GameObject selection = null;
        float distance = 0;

        ICharacterController controller;

        void Start()
        {
            controller = GetComponent<CharacterModel>().ControllerContainer.Result;
        }

        void Update()
        {
            SendPlayerPositionToDialogUICanvas();
            SendMessageToUsable();
            StopConversationIfPlayerMoves();
        }

        /// <summary>
        /// Sends the player position to dialogUI Canvas
        /// Since the player can move, the player dialog should move with him/her
        /// </summary>
        void SendPlayerPositionToDialogUICanvas()
        {
            Vector2 dialogPosition = (Vector2)bodyCollider.bounds.center + new Vector2(0,(bodyCollider.bounds.size.y / 2));
            dialogUICanvas.UpdatePlayerDialogPosition(dialogPosition);
        }

        /// <summary>
        /// Determines whether the mouse pointer is over UI gameobject.
        /// </summary>
        /// <returns><c>true</c> if the mouse pointer is over UI gameobject; otherwise, <c>false</c>.</returns>
        bool IsPointerOverGameObject()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the interact button is down.
        /// </summary>
        /// <returns><c>true</c> if interact button is down; otherwise, <c>false</c>.</returns>
        bool IsInteractButtonDown()
        {
            // Check for use key or button (only if releasing button on samSelectedUsableObjecte selection:
            return Input.GetKeyDown(KeyCode.Mouse0);
               
        }

        /// <summary>
        /// Sends "OnUse" message to the usable(NPC) component
        /// First it checks whether any Usable component is within range
        /// </summary>
        void SendMessageToUsable()
        {
            
            // If the player presses the use key/button on a target:
            if(IsInteractButtonDown())
            {
                if(IsPointerOverGameObject())
                {
                    return;
                }

                FindUsable();

                if (usable == null)
                {
                    if(dialogUICanvas.IsResponseMenuActive())
                    {
                        DialogueManager.StopConversation();
                    }
                   
                    return;
                }

                if (distance <= usable.maxUseDistance)
                {
                    DialogueManager.StopConversation();

                    // If within range, send the OnUse message:
                    var fromTransform = transform;
                    if (broadcastToChildren)
                    {
                        usable.gameObject.BroadcastMessage("OnUse", fromTransform, SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
                        usable.gameObject.SendMessage("OnUse", fromTransform, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }


        }

        /// <summary>
        /// Runs raycast within the scene to determine if any Usable component is within range
        /// Based on the Run2DRaycast function from the Selector script within the "Dialogue System For Unity" plugin
        /// It can RaycastAll or the first item only
        /// </summary>
        void FindUsable()
        {
            if (raycastAll)
            {
                RaycastHit2D[] hits;
                hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, maxSelectionDistance, layerMask);
                bool foundUsable = false;
                for(int i=0; i<hits.Length; i++)
                {
                    float hitDistance = Vector3.Distance(gameObject.transform.position, hits[i].collider.transform.position);
                    if (selection == hits[i].collider.gameObject)
                    {
                        foundUsable = true;
                        distance = hitDistance;
                        break;
                    }
                    else
                    {
                        Usable hitUsable = hits[i].collider.gameObject.GetComponentInParent<Usable>();
                        if (hitUsable != null)
                        {
                            foundUsable = true;
                            distance = hitDistance;
                            usable = hitUsable;
                            selection = hits[i].collider.gameObject;
                            if (SelectedUsableObject != null)
                            {
                                SelectedUsableObject(usable);
                            }

                            break;
                        }
                    }
                }

                if (!foundUsable)
                {
                    DeselectTarget();
                }

            }
            else
            {

                // Cast a ray and see what we hit:
                RaycastHit2D hit;
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, maxSelectionDistance, layerMask);
                if (hit.collider != null)
                {
                    distance = Vector3.Distance(gameObject.transform.position, hit.collider.transform.position);
                    if (selection != hit.collider.gameObject)
                    {
                        Usable hitUsable = hit.collider.gameObject.GetComponentInParent<Usable>();
                        if (hitUsable != null)
                        {
                            usable = hitUsable;
                            selection = hit.collider.gameObject;
                            if (SelectedUsableObject != null)
                            {
                                SelectedUsableObject(usable);
                            }
                               
                        }
                        else
                        {
                            DeselectTarget();
                        }
                    }
                }
                else
                {
                    DeselectTarget();
                }
            }
        }

        /// <summary>
        /// Stops the conversation if player moves.
        /// </summary>
        void StopConversationIfPlayerMoves()
        {
            if(!DialogueManager.IsConversationActive)
            {
                return;
            }
            bool hasPlayerMoved = controller.HorizontalAxisDegree != 0 || controller.VerticalAxisDegree != 0;
            if(!hasPlayerMoved)
            {
                return;
            }

            if(dialogUICanvas.IsResponseMenuActive())
            {
                DialogueManager.StopConversation();
            }
        }

        /// <summary>
        /// Deselects the target and nullifies usable and selection
        /// </summary>
        void DeselectTarget()
        {
            if ((usable != null) && (DeselectedUsableObject != null))
            {
                DeselectedUsableObject(usable);
            }
              
            usable = null;
            selection = null;
        }
    }
}