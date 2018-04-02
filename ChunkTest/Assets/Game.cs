using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
	public ObjectiveManager Objectives { get; set; }

	public Game(string campaignName)
	{
		Objectives = ObjectiveManager.Instance;
		Objectives.Load (campaignName);
	}
}