using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// Temporary class for switching scenes
/// CharacterPhysics scene clearly demonstrates all the physics behaviors
/// TargetJumpTest scene is for testing the targeted jump functionality - This was made a different scene to test whether the player goes to 
/// the proper location or not. It's hard to determine it with a dynamic camera. The functionality still works however, with dynamic camera.
/// </summary>
public class SwitchScenes : MonoBehaviour, IPointerClickHandler
{
    public enum TestScenes{DynamicCamera,TargetedJump};
    public TestScenes testScene;

    void LoadScene()
    {
        if (testScene == TestScenes.DynamicCamera)
        {
            SceneManager.LoadScene("CharacterPhysics");
            return;
        }

        SceneManager.LoadScene("TargetedJumpTest");
    }

#region IPointerClickHandler implementation

    public void OnPointerClick(PointerEventData eventData)
    {
        LoadScene();
    }

#endregion
}
