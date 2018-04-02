using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
	private Objective[] objectives;
	private Objective[] activeObjectives;
	private Text[] objectiveLabels;

	private int objectiveIndex;
	private bool inSubObjectives;

	private static ObjectiveManager instance;

	public static ObjectiveManager Instance{ get; private set; }

	private void Awake()
	{
		if (instance == null)
		{
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} else
			Destroy (gameObject);
	}

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

	public void NextObjective()
	{
		if (inSubObjectives)
			return;

		objectiveIndex++;

		activeObjectives = new Objective[] { objectives [objectiveIndex] };

		if (objectives [objectiveIndex].objectives != null)
		{
			activeObjectives [0].isParent = true;

			System.Array.Resize (ref activeObjectives, objectives [objectiveIndex].objectives.Length + 1);
			objectives [objectiveIndex].objectives.CopyTo (activeObjectives, 1);
			inSubObjectives = true;
		}

		if (objectiveLabels != null)
		{
			foreach (Text t in objectiveLabels)
				Destroy (t);
		}

		objectiveLabels = new Text[activeObjectives.Length];

		int i = 0;
		int indent = 0;
		foreach (Objective o in activeObjectives)
		{
			GameObject g = (GameObject)Instantiate(Resources.Load("UI/ObjectiveTxt"), new Vector3(0, 0, 0), Quaternion.identity);
			Text t = g.GetComponent<Text> ();
			t.rectTransform.SetParent (GameObject.Find ("ObjectivePanel").transform);
			t.rectTransform.anchoredPosition = new Vector2 (0, 0);
			t.rectTransform.localPosition = new Vector3 (10 + indent, (-10 * i) - (t.fontSize * (i + 1)), 0);
			t.text = o.display + " (0/" + o.criteria.amount + ")";

			objectiveLabels [i] = t;

			if (o.isParent)
				indent = 20;
			else if (!o.isParent && i + 1 < activeObjectives.Length)
			{
				if (activeObjectives [i + 1].isParent)
					indent = 0;
			}

			i++;
		}
	}

	public void ObjectiveCheck(string category, string item, int achieved)
	{
		if (item == "DefaultItem")
			return;

		int iterationCount = 0;
		int i = 0;
		foreach (Objective obj in activeObjectives)
		{
			if (obj.achieved || obj.isParent)
			{
				i++;
				continue;
			}

			if (obj.criteria.category == category && obj.criteria.item == item)
			{
				obj.criteria.amountAchieved = achieved;
				if (achieved >= obj.criteria.amount)
				{
  					objectiveLabels [i].text = obj.display + " Complete!";
					obj.achieved = true;
					continue;
				}
				else
					objectiveLabels [i].text = obj.display + " (" + achieved + "/" + obj.criteria.amount + ")";
			}

			i++;
			iterationCount++;
		}

		if (iterationCount == 0 && inSubObjectives)
		{
			inSubObjectives = false;
			objectives [objectiveIndex].achieved = true;
			objectiveLabels [0].text = objectives [objectiveIndex].display + " Complete!";
		}
	}

	private void onComplete()
	{

	}
}