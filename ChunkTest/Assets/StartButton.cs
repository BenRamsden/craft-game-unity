using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

	void Start () {
		Button btn = gameObject.GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
	}
	
	void TaskOnClick () {
		Debug.Log ("You clicked the button");

		GameObject world = GameObject.Find ("World");
		WorldGenerator wg = world.GetComponent<WorldGenerator> ();
		wg.InitPlayer ();

		GameObject menuCamera = GameObject.Find ("MenuCamera");
		Destroy (menuCamera);

		GameObject canvas = GameObject.Find ("Canvas");
		Destroy (canvas);
	}
}
