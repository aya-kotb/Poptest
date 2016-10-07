using UnityEngine;
using System.Collections;

public class PlayerProfileLanguageExample : MonoBehaviour
{
    public static languages languageCode = languages.english;
    public static string island;
    public static string scene;
    public static float dialogSpeed;
    [SerializeField]
    languages initLanguage;
    [SerializeField]
    string initIsland;
    [SerializeField]
    [Range(5, 15)]
    float initDialogSpeed;

    void Awake()
    {
        island = initIsland;
        dialogSpeed = initDialogSpeed;
        languageCode = initLanguage;
    }

    public enum languages
    {
        english,
        spanish,
        french
    };

    const string ENGLISH = "en";//default
    const string SPANISH = "es";
    const string FRENCH = "fr";

    static public string preferedLanuage
    {
        get
        {
            string language;
            switch (languageCode)
            {
                case languages.spanish:
                    language = SPANISH;
                    break;
                case languages.french:
                    language = FRENCH;
                    break;
                default:
                    language = ENGLISH;
                    break; 
            }
            return language;
        }
    }
}
