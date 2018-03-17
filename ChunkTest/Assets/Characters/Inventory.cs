using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	private List<Item>[] inventoryList = {
		new List<Item>(64), new List<Item>(64), new List<Item>(64), new List<Item>(64),
		new List<Item>(64), new List<Item>(64), new List<Item>(64), new List<Item>(64),
		new List<Item>(64), new List<Item>(64), new List<Item>(64), new List<Item>(64),
		new List<Item>(64), new List<Item>(64), new List<Item>(64), new List<Item>(64),
		new List<Item>(64), new List<Item>(64), new List<Item>(64), new List<Item>(64)
	};

	private ItemBar itemBar;



	public void Start(){

	}

	public void Delete(){
		foreach(List<Item> list in inventoryList){
			list.Clear();
		}
		itemBar.Delete();
	}

	public void addBlock(Block block){
		if (!itemBar.isFull()) {
			itemBar.addBlock(block);
		} else {
			//addblocktoinventory
		}
	}

	public string removeBlock(){
		return itemBar.removeBlock();
	}


}
