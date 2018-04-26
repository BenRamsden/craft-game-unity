using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler{
	Vector3 originalPosition;
	bool dragStart = false;

	/// <summary>
	/// Event listener for beginning to drag an item in the inventory.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnDrag(PointerEventData eventData){
		if (!dragStart) {
			originalPosition = transform.localPosition;
			dragStart = true;
		}
		transform.position = Input.mousePosition;
		GameObject.Find("Main_Character").GetComponent<Inventory>().dragEnd(this.gameObject);
	}

	/// <summary>
	/// Event listener for ending the drag of an item in the inventory.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnEndDrag(PointerEventData eventData){
		transform.localPosition = originalPosition;
		dragStart = false;
	}
}
