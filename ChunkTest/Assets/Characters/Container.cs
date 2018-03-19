using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container{
	private List<Item>[] container;
	private Text[] slotItemCounts;
	private Image[] slotItemImages;
	private int containerSize;

	public readonly static int SLOT_SIZE = 64;

	public Container(int containerSize, string name){
		this.containerSize = containerSize;

		container = new List<Item>[containerSize];
		slotItemCounts = new Text[containerSize];
		slotItemImages = new Image[containerSize];

		slotItemCounts = GameObject.Find(name).GetComponentsInChildren<Text>();
		slotItemImages = GameObject.Find(name).GetComponentsInChildren<Image>();

		for (int i = 0; i < containerSize; i++) {
			container[i] = new List<Item>(64);
		}
	}

	public bool isFull(){
		foreach(List<Item> list in container){
			if (list.Count < SLOT_SIZE)
				return false;
		}
		return true;
	}

	public bool addItem(Item item){
		int currentIndex = checkIfListFree(item.resourceString);
		if(currentIndex != -1){
			container [currentIndex].Add(item);
			return true;
		}
		return false;
	}

	public string removeItem(int slot){
		int currentSlotSize = container[slot].Count;
		List<Item> currentSlot = container[slot];

		if (currentSlotSize > 0) {
			string typeOfBlock = currentSlot[currentSlotSize - 1].resourceString;
			currentSlot.RemoveAt(currentSlotSize - 1);
			return typeOfBlock;
		}
		return null;
	}

	public void setUI(){
		string materialName;
		int currentSlotSize = 0;

		for (int i = 0; i < containerSize; i++) {
			currentSlotSize = container[i].Count;
			materialName = (currentSlotSize > 0)? (container[i])[0].resourceString : "DefaultItem";

			slotItemCounts [i].text = currentSlotSize.ToString ();
			slotItemImages[i].material = (Material)Resources.Load(string.Concat("Menu/",materialName));
		}
	}

	private int checkIfListFree(string itemName){
		int index = -1;
		int currentSlotSize = 0;
		for(int i = 0; i < containerSize; i++){
			currentSlotSize = container[i].Count;
			if (currentSlotSize == 0) {
				index = i;
			}else if (currentSlotSize < SLOT_SIZE) {
				if ((container [i]) [0].resourceString == itemName) {
					index = i;
					break;
				}
			}

		}
		return index;
	}
}