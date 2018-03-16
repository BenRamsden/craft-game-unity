using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item{
	protected GameObject thisBody;
	protected Vector3 worldPosition;
	public string resourceString { get; set; }

	public void draw(){
		if (thisBody == null)
		{
			thisBody = GameObject.Instantiate(Resources.Load(resourceString), worldPosition , Quaternion.identity) as GameObject;
			thisBody.name = resourceString;
		}
	}

	public void Delete(){
		if (thisBody != null) {
			GameObject.Destroy (thisBody);
			thisBody = null;
		}
	}
}
