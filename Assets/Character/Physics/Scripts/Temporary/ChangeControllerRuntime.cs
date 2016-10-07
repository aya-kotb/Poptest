using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Poptropica2.Characters;

/// <summary>
/// This class a temporary class for changing controllers during runtime
/// </summary>
public class ChangeControllerRuntime : MonoBehaviour, IPointerClickHandler
{
    public ICharacterControllerContainer controllerContainer;

    #region IPointerClickHandler implementation

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.GetInstance().DispatchOnOverrideController(controllerContainer.Result);
    }

    #endregion

    [System.Serializable]
    public class ICharacterControllerContainer : IUnifiedContainer<ICharacterController>
    {

    }
}
