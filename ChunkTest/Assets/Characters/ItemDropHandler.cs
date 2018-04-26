using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler{

	/// <summary>
	/// Event listener for dropping an item in the inventory.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnDrop(PointerEventData eventData){
		RectTransform containerBoundary = transform as RectTransform;
		GameObject.Find ("Main_Character").GetComponent<Inventory> ().itemDrop();
	}
}
