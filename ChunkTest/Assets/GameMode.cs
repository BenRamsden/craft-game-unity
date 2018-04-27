using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour {

	// Use this for initialization
	void Start() {
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	/// <summary>
	/// Raises the on click event.
	/// </summary>
	void TaskOnClick() {
		Debug.Log("You have clicked the button!");
		SceneManager.LoadScene("scene1", LoadSceneMode.Single);
	}

}
