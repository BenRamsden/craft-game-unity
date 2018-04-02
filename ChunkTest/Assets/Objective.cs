using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective
{
	public string name;
	public string display;
	public string on_complete;
	public Criteria criteria;
	public Objective[] objectives;

	public bool achieved;
	public bool isParent;
}

[System.Serializable]
public class Criteria
{
	public string category;
	public string item;
	public int amount;
	public int amountAchieved;
}

[System.Serializable]
public class Objectives
{
	public Objective[] objectives;
}