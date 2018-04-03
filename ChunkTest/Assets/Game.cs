using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public static Game Instance{ get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} 
	}

	public void Reset()
	{
		ObjectiveManager.Instance.Reset ();
	}

	public void LoadCampaign(string campaignName)
	{
		ObjectiveManager.Instance.Load (campaignName);

		ObjectiveManager.Instance.ObjectiveCompleteHandlers.Add ("game", GameComplete);
	}

	private void GameComplete(string item, int amount)
	{
		switch (item)
		{
		case "end":
			ShowCredits ();
			break;
		}
	}

	public void ShowCredits()
	{
		Animator a = (Animator)GameObject.Find ("CreditsFade").GetComponent ("Animator");
		a.enabled = true;
	}

	private void LoadCreditsScene()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("credits");
	}

	private void LoadGameScene()
	{
		Cursor.lockState = CursorLockMode.None;
		Reset ();
		Destroy (GameObject.Find ("World"));
		UnityEngine.SceneManagement.SceneManager.LoadScene ("scene1");
	}
}