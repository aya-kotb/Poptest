using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Poptropica2
{
	/// <summary>
	/// Game configuration. This is the main configuration for the game.
	/// If there are non service related elements that should go here for configuration
	/// such as asset URLs or whatnot, feel free to add them, but note that you will
	/// have to create a custom editor for them in GameConfigurationEditor.
	/// </summary>
	[CreateAssetMenu(fileName="Game", menuName="Game Configuration", order=1)]
	public class GameConfiguration : ScriptableObject {
		public List<ServiceConfiguration> services;

		public void CreateServices(SAMApplication application)
		{
			for(int x = 0; x < services.Count; ++x)
			{
				services[x].CreateService(application);
			}
		}
	}

}
