using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campaign
{
	public ObjectiveManager Objectives { get; set; }

	public Campaign(string campaignName)
	{
		Objectives = ObjectiveManager.Instance;
		Objectives.Load (campaignName);
	}
}