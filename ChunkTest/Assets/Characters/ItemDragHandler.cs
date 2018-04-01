using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler{
	Vector3 originalPosition;
	bool dragStart = false;
	public void OnDrag(PointerEventData eventData){
		if (!dragStart) {
			originalPosition = transform.localPosition;
			dragStart = true;
		}
		transform.position = Input.mousePosition;
		GameObject.Find("Main_Character").GetComponent<Inventory>().dragEnd(this.gameObject);
	}

	public void OnEndDrag(PointerEventData eventData){
		transform.localPosition = originalPosition;
		dragStart = false;
	}
}
