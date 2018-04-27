using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	public static Game Instance{ get; private set; }

	/// <summary>
	/// Sets a static reference to itself.
	/// </summary>
	private void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} 
	}

	/// <summary>
	/// Reset this instance.
	/// </summary>
	public void Reset() {
		ObjectiveManager.Instance.Reset ();
	}

	/// <summary>
	/// Loads the campaign.
	/// </summary>
	/// <param name="campaignName">Campaign name.</param>
	public void LoadCampaign(string campaignName) {
		ObjectiveManager.Instance.Load (campaignName);
		ObjectiveManager.Instance.ObjectiveCompleteHandlers.Add ("game", GameComplete);
	}

	/// <summary>
	/// Handles completion of the game.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="amount">Amount.</param>
	private void GameComplete(string item, int amount) {
		switch (item) {
		case "end":
			ShowCredits ();
			break;
		}
	}

	/// <summary>
	/// Shows the credits.
	/// </summary>
	public void ShowCredits() {
		Animator a = (Animator)GameObject.Find ("CreditsFade").GetComponent ("Animator");
		a.enabled = true;
	}

	/// <summary>
	/// Loads the credits scene.
	/// </summary>
	private void LoadCreditsScene() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("credits");
	}

	/// <summary>
	/// Loads the game scene.
	/// </summary>
	private void LoadGameScene() {
		Cursor.lockState = CursorLockMode.None;
		Reset ();
		Destroy (GameObject.Find ("World"));
		UnityEngine.SceneManagement.SceneManager.LoadScene ("scene1");
	}
}