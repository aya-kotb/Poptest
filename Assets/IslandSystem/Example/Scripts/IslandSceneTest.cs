using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
    public class IslandSceneTest : MonoBehaviour {

        // Use this for initialization
        void Start () {
            Debug.Log(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
        
        // Update is called once per frame
        public void OnClickMapButton () {
//            SceneLoadManager.LoadScene(SceneLoadManager.previousScene);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
