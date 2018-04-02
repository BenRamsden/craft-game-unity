using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	private Container mainBag;
	private Container activeBar;
	private Container craftingMatrix;

	private int selectedSlot = 5;

	public void Start(){
		// Add a event to allow objectives to add to the inventory on objective omplete
		ObjectiveManager.Instance.ObjectiveCompleteHandlers.Add ("inventory", objectiveComplete);

		activeBar = new Container(6, "activeBar");
		//mainBag = new Container(20);
		//craftingMatrix = new Container(9);
	}

	public void Delete(){
		
	}

	public bool addItem(Item item){
		if (!activeBar.isFull()) {
			return activeBar.addItem(item);
		} else {
			//AddBlockToMainBag
		}
		return false;
	}

	public string removeItem(){
		return activeBar.removeItem(selectedSlot);
	}

	public void setSelectedSlot(int selectedSlot){
		this.selectedSlot = selectedSlot;
	}

	public void setUI(){
		if(activeBar != null)
			activeBar.setUI();
		//mainBag.setUI();
		//craftingMatrix.setUI();
	}

	/**
	 * Objective complete callback to add item to the inventory
	 * @param string item The item name
	 * @param int amount The amount to add
	 */
	private void objectiveComplete(string item, int amount)
	{
		Block b = new Block ();
		b.resourceString = item;
		for(int i = 0; i < amount; i++)
			addItem (b);
	}
}
