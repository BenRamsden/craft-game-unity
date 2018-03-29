using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectiveManager
{
	public bool Load(string campaign)
	{
		var objectives = Resources.Load("Campaign/" + campaign) as TextAsset;
		Objectives a = JsonUtility.FromJson<Objectives> (objectives.text);

		foreach (Objective o in a.objectives)
		{
			Debug.Log (o.name);
		}
		return true;
	}
}