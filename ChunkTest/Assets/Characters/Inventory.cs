using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	private Container mainBag;
	private Container activeBar;
	private Container craftingMatrix;

	public bool isToggled { get; set;}

	private int selectedSlot = 5;

	public void Start(){
		activeBar = new Container(6, "activeBar");
		mainBag = new Container(20, "inventoryZone");
		craftingMatrix = new Container (9, "craftingZone");

		mainBag.toggle();
		craftingMatrix.toggle();
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
	}
}
