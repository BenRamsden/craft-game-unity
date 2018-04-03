using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objectives
{
	[System.Serializable]
	public class Objective
	{
		public string name;
		public string display;
		public Details[] ongiven;
		public Details[] oncomplete;
		public Details criteria;
		public Objective[] objectives;

		public bool achieved;
		public bool isParent;
	}

	[System.Serializable]
	public class Details
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
}