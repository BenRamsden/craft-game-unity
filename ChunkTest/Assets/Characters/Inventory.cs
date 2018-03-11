using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	private List<Block>[] itemBar = { 
		new List<Block>(64),
		new List<Block>(64),
		new List<Block>(64),
		new List<Block>(64),
		new List<Block>(64),
		new List<Block>(64)
	};
	private int activeList = 5;
	public Text[] amountText = new Text[6];
	public Image[] items = new Image[6];

    public bool addBlock(Block block){
		int itemBarIndex = checkForEntry(block.BlockType);
		if (itemBarIndex != -1) {
			itemBar [itemBarIndex].Add(block);
			return true;
		}
		return false;
    }

	public string removeBlock(){
		int activeCount = itemBar [activeList].Count;
		if (activeCount > 0) {
			string tempName = itemBar [activeList] [0].BlockType;
			(itemBar [activeList]).RemoveAt(activeCount - 1);
			return tempName;
		}
		return null;
    }

	public void Start(){
		for (int i = 0; i < 6; i++) {
			items[i] = GameObject.Find(string.Concat("item",(i+1).ToString())).GetComponent<Image>();
			amountText[i] = GameObject.Find (string.Concat("item",(i+1).ToString())).GetComponentInChildren<Text>();
		}
		setUI ();
	}
	public void Delete() {
		for (int i = 0; i < 6; i++) {
			itemBar [i].Clear ();
		}
		itemBar = null;
	}

    public void setUI() {
		string materialName;
		for(int i = 0; i < 6; i++){
			materialName = (itemBar[i].Count > 0)? ((Block)(itemBar [i]) [0]).BlockType: "DefaultItem";
			amountText[i].text = itemBar[i].Count.ToString();
			items[i].material = (Material)Resources.Load(string.Concat("Menu/",materialName));
			items[i].GetComponent<RectTransform> ().localScale = new Vector3(1,1,1);
		}
		items [activeList].GetComponent<RectTransform> ().localScale = new Vector3(1.15f,1.15f,1.15f);
    }

	public void setActiveList(int activeList){
		this.activeList = activeList;
	}

	private int checkForEntry(string typeString){
		int listNumber = -1;
		for(int i = 0; i < 6; ++i){
			if (itemBar [i].Count > 0) {
				if ((itemBar [i]) [0].GetType () == typeof(Block)) {
					if (((Block)(itemBar [i]) [0]).BlockType == typeString) {
						if ((itemBar [i]).Count < 64) {
							listNumber = i;
							break;
						}
					}
				}
			} else {
				//This is the case if the Array is empty
				listNumber = i;	
			}
		}
		return listNumber;
	}
}
