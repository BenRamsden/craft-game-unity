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
		GameObject world = GameObject.Find ("World");
		WorldGenerator wg = world.GetComponent<WorldGenerator> ();
		wg.StartGame ();
	}
}
