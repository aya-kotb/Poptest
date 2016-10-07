using UnityEngine;
using System.Collections;

/// <summary>
/// Quest mock class.
/// Holding temporary parameters for quest.
/// </summary>
public class Quest 
{
	public string name;
	public int id;
	public string description;
	public bool isCompleted;

	public Quest(string name, int id,string description,bool isCompleted)
	{
		this.name = name;
		this.id = id;
		this.description = description;
		this.isCompleted = isCompleted;
	}
}

/// <summary>
/// Episode mock class.
/// Contains details of episode data.
/// </summary>
public class Episode
{
	public string description;
	public string medallionCount;
	public string episodeTitle;
	public int difficultyLevel;
	public float progress;
}