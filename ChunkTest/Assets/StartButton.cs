using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {
	InputField seedInput;

	// Use this for initialization
	void Start () {
		seedInput = (InputField)GameObject.Find ("WorldSeed").GetComponent ("InputField");
		Button btn = gameObject.GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
	}

	/// <summary>
	/// Raises the on click event.
	/// </summary>
	void TaskOnClick () {
		GameObject world = GameObject.Find ("World");
		WorldGenerator wg = world.GetComponent<WorldGenerator> ();
		wg.StartGame (seedInput.text, gameObject.name == "StartCreative" ? true : false);
	}
}
