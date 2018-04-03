using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using Objectives;

/**
 * A class to manage the game objectives
 */
public class ObjectiveManager : MonoBehaviour
{
	private Objective[] objectives;
	private Objective[] activeObjectives;
	private Text[] objectiveLabels;

	private int objectiveIndex;
	private bool inSubObjectives;

	public delegate void ObjectiveCallback(string item, int amount);

	public Dictionary<string, ObjectiveCallback> ObjectiveLoadHandlers { get; set; }
	public Dictionary<string, ObjectiveCallback> ObjectiveCompleteHandlers { get; set; }

	public static ObjectiveManager Instance{ get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	/**
	 * Resets the objective manager
	 */
	public void Reset()
	{
		Instance = new ObjectiveManager ();
	}

	public ObjectiveManager()
	{
		ObjectiveLoadHandlers = new Dictionary<string, ObjectiveCallback> ();
		ObjectiveLoadHandlers.Add ("objective_sound", objectiveSound);

		ObjectiveCompleteHandlers = new Dictionary<string, ObjectiveCallback> ();
		ObjectiveCompleteHandlers.Add ("objective", objectiveComplete);
		ObjectiveCompleteHandlers.Add ("objective_sound", objectiveSound);
	}

	/**
	 * Loads a campaign objectives JSON file
	 * @param string campaign The objective file name
	 * @returns bool True on success
	 */
	public bool Load(string campaign)
	{
		var objectivesJson = Resources.Load("Campaign/" + campaign) as TextAsset;
		if (objectivesJson == null)
			return false;

		Objectives.Objectives tmp = JsonUtility.FromJson<Objectives.Objectives> (objectivesJson.text);
		objectives = tmp.objectives;

		objectiveIndex = -1;
		inSubObjectives = false;

		return true;
	}

	/**
	 * Advances the current objective. If the player is currently in a objective that has children, it WILL NOT advance.
	 */
	public void NextObjective()
	{
		// If currently in sub-objective, skip
		if (inSubObjectives)
			return;

		objectiveIndex++;

		// Destroy the previous objective UI labels
		if (objectiveLabels != null)
		{
			foreach (Text t in objectiveLabels)
				Destroy (t);
		}

		// If there are no objectives available, say so
		if (objectiveIndex >= objectives.Length || objectives == null)
		{
			objectiveLabels = new Text[] { CreateText("No objectives available") };
			return;
		}

		// Set the current objective as the active one
		activeObjectives = new Objective[] { objectives [objectiveIndex] };

		// If the objective has sub-objectives, set them as active
		if (objectives [objectiveIndex].objectives != null)
		{
			activeObjectives [0].isParent = true;

			System.Array.Resize (ref activeObjectives, objectives [objectiveIndex].objectives.Length + 1);
			objectives [objectiveIndex].objectives.CopyTo (activeObjectives, 1);
			inSubObjectives = true;
		}

		objectiveLabels = new Text[activeObjectives.Length];

		// Creates UI labels for each objective.
		// i = A iteration count to position each objective on a new line
		// indent = A value to indent the label by, used for sub-objectives
		int i = 0;
		int indent = 0;
		foreach (Objective o in activeObjectives)
		{
			objectiveLabels [i] = CreateText(o.display + " (0/" + o.criteria.amount + ")", indent, i);

			if (o.isParent)
				indent = 20;
			else if (!o.isParent && i + 1 < activeObjectives.Length)
			{
				if (activeObjectives [i + 1].isParent)
					indent = 0;
			}

			ParseDetails (o.ongiven, true);

			i++;
		}
	}

	/**
	 * Creates a objective UI label
	 * @param string text The label text
	 * @param int indent The indent value
	 * @param int iteration The iteration value for line placement
	 * @returns Text A text object
	 */
	private Text CreateText(string text, int indent = 0, int iteration = 0)
	{
		GameObject g = (GameObject)Instantiate(Resources.Load("UI/ObjectiveTxt"), new Vector3(0, 0, 0), Quaternion.identity);
		Text t = g.GetComponent<Text> ();
		t.rectTransform.SetParent (GameObject.Find ("ObjectivePanel").transform);
		t.rectTransform.anchoredPosition = new Vector2 (0, 0);
		t.rectTransform.localPosition = new Vector3 (10 + indent, (-10 * iteration) - (t.fontSize * (iteration + 1)), 0);
		t.text = text;
		return t;
	}

	/**
	 * Checks if the values satisfy a objective.
	 * @param string category The category of the objective e.g. inventory, player
	 * @param string item The item of the objective e.g. grass, object name
	 * @param int achieved The amount achieved e.g. 10 grass blocks
	 */
	public void ObjectiveCheck(string category, string item, int achieved)
	{
		if (item == "DefaultItem")
			return;

		int iterationCount = 0; // Used to check if all objectives have been met. 0 = all achieved
		int i = 0; // Used to access the accociated UI label
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
				// If the objective has been achieved, update the label and parse the complete details
				if (achieved >= obj.criteria.amount)
				{
  					objectiveLabels [i].text = obj.display + " Complete!";
					obj.achieved = true;
					ParseDetails (obj.oncomplete);
					continue;
				}
				else
					objectiveLabels [i].text = obj.display + " (" + achieved + "/" + obj.criteria.amount + ")";
			}

			i++;
			iterationCount++;
		}

		// If all sub-objectives met, the parent objective has been met
		if (iterationCount == 0 && inSubObjectives)
		{
			inSubObjectives = false;
			objectives [objectiveIndex].achieved = true;
			objectiveLabels [0].text = objectives [objectiveIndex].display + " Complete!";
			ParseDetails (objectives [objectiveIndex].oncomplete);
		}
	}

	/**
	 * Parse the details for the ongiven and oncomplete details
	 * @param Details[] details Array of detail values
	 * @param bool isOnLoad States if to check the onload details
	 */
	private void ParseDetails(Details[] details, bool isOnLoad = false)
	{
		if (details == null)
			return;

		Dictionary<string, ObjectiveCallback> callbacks = isOnLoad ? ObjectiveLoadHandlers : ObjectiveCompleteHandlers;

		foreach (Details detail in details)
		{
			if (callbacks.ContainsKey (detail.category))
			{
				callbacks [detail.category] (detail.item, detail.amount);
			} else
				Debug.Log ("Not exists: " + detail.category);
		}
	}

	/**
	 * Plays a objective sound
	 * @param string file The sound file
	 * @param int amount
	 */
	private void objectiveSound(string file, int amount)
	{
		this.GetComponent<AudioSource> ().clip = Resources.Load<AudioClip> ("Sounds/" + file);	
		this.GetComponent<AudioSource> ().Play ();
	}

	/**
	 * Handler for the objective complete detail
	 * @param string item The complete item
	 * @param int amount
	 */
	private void objectiveComplete(string item, int amount)
	{
		switch (item)
		{
		case "next":
			NextObjective ();
			break;
		}
	}
}