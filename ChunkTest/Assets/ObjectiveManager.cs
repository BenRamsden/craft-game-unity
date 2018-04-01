using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectiveManager
{
	private Objective[] objectives;
	private Objective[] activeObjectives;

	private int objectiveIndex;
	private bool inSubObjectives;

	public bool Load(string campaign)
	{
		var objectivesJson = Resources.Load("Campaign/" + campaign) as TextAsset;
		if (objectivesJson == null)
			return false;

		Objectives tmp = JsonUtility.FromJson<Objectives> (objectivesJson.text);
		objectives = tmp.objectives;

		objectiveIndex = -1;
		inSubObjectives = false;

		return true;
	}

	public void Next()
	{
		if (inSubObjectives)
			return;

		objectiveIndex++;

		if (objectives [objectiveIndex].objectives == null)
			activeObjectives = new Objective[] { objectives [objectiveIndex] };
		else
		{
			activeObjectives = objectives [objectiveIndex].objectives;
			inSubObjectives = true;
		}
	}

	public void ObjectiveCheck(string category, string item, int achieved)
	{
		int iterationCount = 0;
		foreach (Objective obj in activeObjectives)
		{
			if (obj.achieved)
				continue;

			if (obj.criteria.category == category && obj.criteria.item == item)
			{
				obj.criteria.amountAchieved = achieved;
				if (obj.criteria.amount == achieved)
				{
					obj.achieved = true;
					continue;
				}
			}

			iterationCount++;
		}

		if (iterationCount == 0 && inSubObjectives)
		{
			inSubObjectives = false;
			objectives [objectiveIndex].achieved = true;
		}
		else if (iterationCount == 0)
		{

		}
	}

	private void onComplete()
	{

	}
}