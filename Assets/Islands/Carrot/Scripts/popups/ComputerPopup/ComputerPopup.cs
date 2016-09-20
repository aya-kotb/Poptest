using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Poptropica2.PopupSystem;

/// <summary>
/// Computer Popup minigame where the user enters a password and command and then plays an asteroid game.
/// </summary>
public class ComputerPopup : PopupNonUI
{
	// specific to this popup
	public GameObject keyboard;
	public GameObject textCanvas;
	public Text screenText;
	public GameObject caret;
	public GameObject launchAnim;
	public RabbotAnimation rabbotScript;
	public GameObject asteroidGame;
	public GameObject message;
	public Text messageText;
    public Sprite keyButton;
    public Sprite keyButtonDown;

	// debugging string
	private string debugMode = "off"; 		        // can be "off", "launch" or "game"

	// game flags
	[HideInInspector]public bool gameOver = false;	// when game is over

	// keyboard variables
	private bool keyboardVisible = false; 			// flag for when keyboard is on screen
	private float keyboardYStart; 					// starting y position when off screen
	private const float keyboardOnscreenY = -3.5f; 	// y position when placed on screen
    private GameObject backScreen;                  // back screen game object

	// text entry variables
	private string stage; 							// popup stage: entering password, entering command
	private string userEntered = ""; 		        // text that user has entered
	private bool animatingText = false; 			// when text is animating on screen
	private int numLines = 0; 						// current number of lines of text
	private const int maxLines = 4; 				// maximum number of lines allowed on screen
	private const float lineHeight = 0.8f; 		    // height between lines of text

	// caret variables
	private float startCaretX; 						// starting position of flashing caret
	private bool blinkOn = true; 					// whether caret is visible
	private int countFrames = 0; 					// number of frames counted for caret
	private int blinkFrames = 30; 					// how many frames for caret to blink on or off

	// text constants
	private const string enterPassword				= "enter password> ";
	private const string correctPassword 			= "fuzzybunny";
	private const string enterCommand 				= "command> ";
	private const string correctCommand 			= "launch rabbot";
	private const string passwordAccepted 			= "...password accepted";
	private const string passwordDenied 			= "...password not recognized";
	private const string commandAccepted 			= "...robot control initiated";
	private const string commandDenied 			= "...command not recognized";

	// Dr. Hare messages
	private float messageDelay 						= 4f; // number of seconds for message to display
	private const string message1 					= "You fool! Watch\n\rwhere you're going!";
	private const string message2 					= "What are you doing!?\n\rYou'll destroy my Rabbot!";
	private const string message3 					= "You'll pay for this!!!";
	private const string message4 					= "Aaaaaaaah!!!!";

    // Don't use Awake or else the Awake function in PopupBase won't be called.
	void Start()
	{
        // get backscreen game object
        backScreen = transform.Find("back_screen").gameObject;

		// hide launch animation, asteroid game and messaging display
		launchAnim.SetActive (false);
		asteroidGame.SetActive (false);
		message.SetActive (false);

        // set initial stage to entering password
		stage = enterPassword;

		// get start position for keyboard off screen
        keyboardYStart = keyboard.transform.localPosition.y;

		// animate keyboard onto screen
		AnimateKeyboardIn ();

		// get starting caret x position
		startCaretX = caret.transform.localPosition.x;

		// display password text animation
		StartCoroutine (AnimateText (enterPassword, 2));
	}

	void Update()
    {
		// make caret blink
		countFrames++;
		// if reached number of blink frames, then alternate states
		if (countFrames > blinkFrames)
        {
			countFrames = 0;
			if (blinkOn)
            {
				blinkOn = false;
				caret.SetActive (false);
			}
            else
            {
				blinkOn = true;
				caret.SetActive (true);
			}
		}
	}

    /// <summary>
    /// Animates the keyboard onto the screen.
    /// </summary>
	public void AnimateKeyboardIn()
    {
		if (!keyboardVisible)
        {
			keyboardVisible = true;
			LeanTween.moveY(keyboard, keyboardOnscreenY, 1f).setEase(LeanTweenType.easeOutBounce);
            backScreen.GetComponent<BoxCollider2D>().enabled = false;
		}
	}
	
    /// <summary>
    /// Animates the keyboard off screen.
    /// </summary>
	public void AnimateKeyboardOut()
    {
		if (keyboardVisible)
        {
			keyboardVisible = false;
			LeanTween.moveY (keyboard, keyboardYStart, 1f).setEase (LeanTweenType.easeOutBack);
            backScreen.GetComponent<BoxCollider2D>().enabled = true;
		}
	}

    /// <summary>
    /// Animates typed text.
    /// </summary>
    /// <param name="text">Text to animate.</param>
    /// <param name="startDelay">Start delay.</param>
    /// <param name="launch">If set to <c>true</c> launch rabbot ship.</param>
	IEnumerator AnimateText(string text, float startDelay = 0f, bool launch = false)
    {
		
		// if not already animating
		if (!animatingText)
        {
			animatingText = true;

			// initial delay if any
			if (startDelay != 0f)
				yield return new WaitForSeconds (startDelay);

			// animate one letter at a time
			int length = text.Length;
			for (int i = 0; i != length; i++)
            {
				string letter = text.Substring (i, 1);
				screenText.text = screenText.text + letter;

				// if line return, then jump to next line
				if (letter == "\r")
					GoNextLine ();
				else // else move caret to right based on letter width
                    caret.transform.localPosition = new Vector3 (caret.transform.localPosition.x + GetLetterWidth(letter), caret.transform.localPosition.y, 0f);

				// delay between letters
				yield return new WaitForSeconds (0.1f);
			}

			animatingText = false;

			// if launching rabbot
			if (launch)
				DoLaunch ();
		}
	}

    /// <summary>
    /// User types one letter.
    /// </summary>
    /// <param name="keyValue">Key value.</param>
	public void TypeLetter(string keyValue)
    {
        // debug modes
        if (debugMode == "launch")
        {
            DoLaunch ();
        }
        else if (debugMode == "game")
        {
            StartGame();
        }

		// if not animating text
		if (!animatingText)
        {
			// check key value
			switch (keyValue)
            {
    			case "delete":

    				// if letters to delete
    				if (userEntered.Length != 0)
                    {
    					// get last letter typed
    					string letterRemoved = userEntered.Substring (userEntered.Length - 1, 1);
    					// remove last letter from user entered text
    					userEntered = userEntered.Substring (0, userEntered.Length - 1);
    					// move last letter from text field on screen
    					screenText.text = screenText.text.Substring (0, screenText.text.Length - 1);
    					// move caret to left based on letter width
                        caret.transform.localPosition = new Vector3 (caret.transform.localPosition.x - GetLetterWidth(letterRemoved), caret.transform.localPosition.y, 0f);
    				}
    				return;

    			case "space":
    				keyValue = " ";
    				break;

    			case "enter":
    				// if nothing entered then skip out
    				if (userEntered == "")
    					return;
    				
    				// add new line
    				screenText.text = screenText.text + "\n\r";
    				GoNextLine ();

    				// check stage and user entered text
    				if (stage == enterPassword)
                    {
    					if (userEntered == correctPassword) {
    						stage = enterCommand;
    						StartCoroutine (AnimateText (passwordAccepted + "\n\r" + enterCommand));
    					}
                        else
                        {
    						StartCoroutine (AnimateText (passwordDenied + "\n\r" + enterPassword));
    					}
    				}
                    else
                    {
    					if (userEntered == correctCommand)
                        {
    						StartCoroutine (AnimateText (commandAccepted, 0f, true));
    					}
                        else
                        {
    						StartCoroutine (AnimateText (commandDenied + "\n\r" + enterCommand));
    					}
    				}
    				// clear user entered text
    				userEntered = string.Empty;
    				return;
			}

			// add letter to user entered text
			userEntered += keyValue;
			// add letter to text on screen
			screenText.text = screenText.text + keyValue;
			// update caret position based on letter width
            caret.transform.localPosition = new Vector3 (caret.transform.localPosition.x + GetLetterWidth(keyValue), caret.transform.localPosition.y, 0f);
		}
	}

    /// <summary>
    /// Goes to the next line on screen.
    /// </summary>
	private void GoNextLine()
    {
		numLines++;
		// if reached maximum number of lines
		if (numLines >= maxLines)
        {
			// move text up two lines
			numLines -= 2;
			// move caret up one line and to starting position
            caret.transform.localPosition = new Vector3 (startCaretX, caret.transform.localPosition.y + lineHeight, 0f);
			// remove last two lines from onscreen text
			for (int i = 0; i != 2; i++)
            {
				int pos = screenText.text.IndexOf ("\r");
				screenText.text = screenText.text.Substring (pos + 1);
			}
		}
        else
        {
			// move caret to next line
            caret.transform.localPosition = new Vector3 (startCaretX, caret.transform.localPosition.y - lineHeight, 0f);
		}
	}

    /// <summary>
    /// Gets the width of the letter.
    /// </summary>
    /// <returns>The letter width.</returns>
    /// <param name="letter">Letter.</param>
	private float GetLetterWidth( string letter)
    {
		float width;
		switch (letter)
		{
    		case "m":
    		case "w":
    			width = 11;
    			break;

    		case ">": // add extra width for > to make sure caret is far enough to the right
    			width = 8;
    			break;

    		case "a":
    		case "b":
    		case "c":
    		case "d":
    		case "e":
    		case "g":
    		case "h":
    		case "n":
    		case "o":
    		case "p":
    		case "q":
    		case "u":
    		case "v":
    		case "x":
    		case "y":
    		case "z":
    			width = 7;
    			break;

    		case "k":
    		case "s":
    			width = 6;
    			break;

    		case "f":
    		case "r":
    		case "t":
    			width = 5;
    			break;

    		case "j":
    			width = 4;
    			break;

    		case "i":
    		case "l":
    		case ".":
    		case " ":
    			width = 3;
    			break;

            default:
    			width = 0;
    			break;
		}

		// width is based on font size of 16
		return (width * 16 / 320);
	}

    /// <summary>
    /// Plays launch animation/
    /// </summary>
	private void DoLaunch()
    {
		// hide text
		textCanvas.SetActive (false);
		// hide keyboard
		keyboard.SetActive (false);
		// show rabbot
		launchAnim.SetActive (true);
		// trigger launch
		rabbotScript.DoLaunch();
	}

    /// <summary>
    /// Starts the asteroid game.
    /// </summary>
	public void StartGame()
    {
		// hide text
		textCanvas.SetActive (false);
		// hide keyboard
		keyboard.SetActive (false);
		// show game
		asteroidGame.SetActive (true);
	}

    /// <summary>
    /// Collides with the asteroid.
    /// </summary>
    /// <param name="hitCounter">Hit counter.</param>
	public void CollideAsteroid (int hitCounter)
    {
		
		// check number of hits and update messaging
		switch (hitCounter) {
		case 1:
			messageText.text = message1;
			break;
		case 2:
			messageText.text = message2;
			break;
		case 3:
			messageText.text = message3;
			break;
		case 4:
			messageText.text = message4;
			break;
		}
		StartCoroutine (displayMessage ());
	}

    /// <summary>
    /// Displaies the message.
    /// </summary>
    /// <returns>The message.</returns>
	IEnumerator displayMessage()
    {
		// turn on and scale up messaging
		message.SetActive (true);
		message.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
		LeanTween.scale(message, new Vector3 (1f, 1f, 1f), 1f).setEase(LeanTweenType.linear);
		yield return new WaitForSeconds(messageDelay);
		// scale down after pause
		LeanTween.scale(message, new Vector3 (0.01f, 0.01f, 0.01f), 1f).setEase(LeanTweenType.linear).setOnComplete(DoneMessage);
	}

    /// <summary>
    /// When message animation is done.
    /// </summary>
	private void DoneMessage()
    {
		// hide messaging
		message.SetActive (false);
	}
}