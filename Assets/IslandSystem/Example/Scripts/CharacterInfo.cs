using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	public class CharacterInfo
	{

		public string characterID;

		public CharacterInfo ()
		{
			Debug.Log ("Cheracter Info");
			AuthenticateUser ();
		}

		void AuthenticateUser ()
		{
			GameSparksManager.Instance ().Authenticate ("sujil_01", "12345", HandleonAuthSuccess, HandleonAuthFailed);
		}

		void GetCharacterID ()
		{
			GameSparksManager.Instance ().CreateCharacter ("Red Flame", "M", HandleonCharacterCreated, HandleonRequestFailed);
		}

		void HandleonCharacterCreated (string newCharacterID)
		{
			Debug.Log ("Character ID: " + newCharacterID);
			characterID = newCharacterID;
		}

		void HandleonRequestFailed (GameSparksError error)
		{
			Debug.LogError ("Create Character request failed: " + error.errorMessage);
		}
		
		void HandleonAuthSuccess (AuthResponse authResponse)
		{
			GetCharacterID ();
		}
		
		void HandleonAuthFailed (AuthFailed error)
		{
			Debug.LogError ("Authentication Failed: " + error.errorMessage);
		}
	}
}
