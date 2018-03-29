using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective
{
	public string name;
	public string display;
	public string on_complete;
	public string criteria;
}

[System.Serializable]
public class Objectives
{
	public Objective[] objectives;
}