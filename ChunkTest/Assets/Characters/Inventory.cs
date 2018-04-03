using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	private Container mainBag;
	private Container activeBar;
	private Container craftingMatrix;
	private Container craftingProduce;

	public bool isToggled { get; set;}

	enum containerNames {activeBar, mainBag, craftingMatrix, craftingProduce};

	private int selectedSlot = 5;

	public void Start(){
		activeBar = new Container(6, containerNames.activeBar.ToString());
		mainBag = new Container(20, containerNames.mainBag.ToString());
		craftingMatrix = new Container (9, containerNames.craftingMatrix.ToString());
		craftingProduce = new Container (1, containerNames.craftingProduce.ToString ());

		mainBag.toggle();
		craftingMatrix.toggle();
		craftingProduce.toggle();
		isToggled = false;
	}

	public void Delete(){
	}

	public bool addItem(Item item){
		if (!activeBar.isFull()) {
			return activeBar.addItem(item);
		} else {
			return mainBag.addItem(item);
		}
		return false;
	}

	public void toggleBag(){
		isToggled = mainBag.toggle();
		craftingMatrix.toggle();
		craftingProduce.toggle();
	}

	public string removeItem(){
		return activeBar.removeItem(selectedSlot);
	}

	public void setSelectedSlot(int selectedSlot){
		this.selectedSlot = selectedSlot;
	}

	public void setUI(){
		if (activeBar != null) {
			activeBar.setUI();
		}

		if (mainBag != null) {
			mainBag.setUI();
		}
			
		if (craftingMatrix != null) {
			craftingMatrix.setUI();
		}

		if (craftingProduce != null) {
			craftingProduce.setUI();
		}
	}

	GameObject item;
	public void dragEnd(GameObject item){
		this.item = item;
	}

	public void itemDrop(){
		Block newBlock = new Block();
		bool isBeingDropped = false;
		Container oldContainer = null, newContainer = null;
		int oldContainerIndex = -1, newContainerIndex = -1;

		if (item != null) {
			newBlock.resourceString = item.GetComponent<Image>().material.name;
			int numOfItems = int.Parse(item.GetComponentInChildren<Text>().text);
			string oldContainerName = item.transform.parent.name;

			if (oldContainerName == containerNames.activeBar.ToString ()) {
				oldContainer = activeBar;
			} else if (oldContainerName == containerNames.mainBag.ToString ()) {
				oldContainer = mainBag;
			} else if (oldContainerName == containerNames.craftingMatrix.ToString ()) {
				oldContainer = craftingMatrix;
			} else {
				oldContainer = craftingProduce;
			}

			if (RectTransformUtility.RectangleContainsScreenPoint (craftingMatrix.getBoundary (), Input.mousePosition)) {
				newContainer = craftingMatrix;
			} else if (RectTransformUtility.RectangleContainsScreenPoint (mainBag.getBoundary (), Input.mousePosition)) {
				newContainer = mainBag;
			} else if (RectTransformUtility.RectangleContainsScreenPoint (activeBar.getBoundary (), Input.mousePosition)) {
				newContainer = activeBar;
			} else{
				isBeingDropped = true;
			}

			oldContainerIndex = item.transform.GetSiblingIndex();
			if (newContainer != null) {
				newContainerIndex = newContainer.findSlotThatIsnt(Input.mousePosition, oldContainerIndex);
			}

			if (newContainerIndex != -1) {
				if (!isBeingDropped) {
					newContainer.addItemsAtIndex(newBlock, numOfItems, newContainerIndex);
				} else {
					Debug.Log ("Item Dropped");
				}
				oldContainer.removeItemsAtIndex(oldContainerIndex);
			}
		}
	}
}
