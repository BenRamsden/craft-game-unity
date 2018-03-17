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
		activeBar.setUI();
		//mainBag.setUI();
		//craftingMatrix.setUI();
	}
}
