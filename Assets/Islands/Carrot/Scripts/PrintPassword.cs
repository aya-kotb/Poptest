using UnityEngine;

/// <summary>
/// Prints sheet with password in Loading scene.
/// </summary>
public class PrintPassword:MonoBehaviour
{
	float startY;
	bool printing = false;
	bool printed = false;

	void Start()
	{
		startY = transform.position.y;
		//Here we'd want to check for events to see if the print out has already been printed out or collected.

		//if printed already and not collected
		//Printed();
		//else printed and collected
		//Remove print out from game.
	}

	private void Printed()
	{
		Vector3 position = transform.position;
		position.y = startY - 1;
		transform.position = position;
		printing = false;
		printed = true;
	}

	void Update()
	{
		if(!printed)
		{
			if(printing)
			{
				if(transform.position.y < (startY - 1))
				{
					Printed();
				}
				else
				{
					//Printing out...
					Vector3 position = transform.position;
					position.y -= Time.deltaTime * 0.5f;
					transform.position = position;
				}
			}
			else if(!printing)
			{
				//Check for player
				GameObject player = GameObject.Find("Player");
				if(player)
				{
					//Print out once they get close enough.
					if(Vector3.Distance(transform.position, player.transform.position) < 5.5)
					{
						printing = true;
					}
				}
			}
		}
	}
}