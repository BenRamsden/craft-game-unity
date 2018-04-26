using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	private Container mainBag;
	private Container activeBar;
	private Container craftingMatrix;
	private Container craftingProduce;

	private CraftingHandler ch;

	public bool isToggled { get; set;}

	enum containerNames {activeBar, mainBag, craftingMatrix, craftingProduce};

	private int selectedSlot = 5;

	// Use this for initialization
	public void Start(){
		// Add an event to allow objectives to add to the inventory on objective complete
		ObjectiveManager.Instance.ObjectiveCompleteHandlers.Add ("inventory", objectiveComplete);

		activeBar = new Container(6, containerNames.activeBar.ToString());
		mainBag = new Container(20, containerNames.mainBag.ToString());
		craftingMatrix = new Container (9, containerNames.craftingMatrix.ToString());
		craftingProduce = new Container (1, containerNames.craftingProduce.ToString ());

		mainBag.toggle();
		craftingMatrix.toggle();
		craftingProduce.toggle();
		isToggled = false;

		ch = new CraftingHandler();
		ch.initialise();
	}

	/// <summary>
	/// Adds the item to a container in the inventory in the priority: activeBar > mainBag.
	/// </summary>
	/// <returns><c>true</c>, if item was added, <c>false</c> otherwise.</returns>
	/// <param name="item">Item.</param>
	public bool addItem(Item item){
		if (!activeBar.isFull()) {
			return activeBar.addItem(item);
		} else {
			return mainBag.addItem(item);
		}
		return false;
	}

	/// <summary>
	/// Toggles the inventory's visibility.
	/// </summary>
	public void toggleBag(){
		isToggled = mainBag.toggle();
		craftingMatrix.toggle();
		craftingProduce.toggle();
	}

	/// <summary>
	/// Removes the item at the currently selected slot of the activeBar.
	/// </summary>
	/// <returns>The item.</returns>
	public string removeItem(){
		return activeBar.removeItem(selectedSlot);
	}

	/// <summary>
	/// Sets the selected slot of the activeBar.
	/// </summary>
	/// <param name="selectedSlot">Selected slot.</param>
	public void setSelectedSlot(int selectedSlot){
		this.selectedSlot = selectedSlot;
	}

	/// <summary>
	/// Sets the UI of all of the containers in the inventory.
	/// </summary>
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
	/// <summary>
	/// Upon dropping an item, sets that item in the inventory's memory for later use.
	/// </summary>
	/// <param name="item">Item.</param>
	public void dragEnd(GameObject item){
		this.item = item;
	}

	/// <summary>
	/// Finds the current position of the mouse in relation to the container positions on screen.
	/// Finds the correct container and slot within said container in order to place the item
	/// (referenced in memory by the dragEnd function) at that location.
	/// </summary>
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
				
			if (newContainer != null) {
				if (newContainer == craftingMatrix) {
					Item[] newItems = ch.craftItem(craftingMatrix);
					if (newItems != null) {
						foreach (Item newItem in newItems) {
							craftingProduce.addItem(newItem);
						}
					}
				}
			}
		}
	}
		
	private void objectiveComplete(string item, int amount){
		Block b = new Block ();
		b.resourceString = item;
		for(int i = 0; i < amount; i++)
			addItem (b);
	}
}
