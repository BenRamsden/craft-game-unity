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

	public Text[] amountText = new Text[6];
	public Image[] items = new Image[6];

    public bool addBlock(Block block){
		int itemBarIndex = checkForEntry(block.BlockType);
		if (itemBarIndex != -1) {
			itemBar [itemBarIndex].Add (block);
			return true;
		}
		return false;
    }

	public Block getBlock(string typeString){
		int itemBarIndex = checkForEntry(typeString);
		Debug.Log (itemBarIndex.ToString());
		if (itemBarIndex != -1) {
			return (Block) itemBar[itemBarIndex][itemBar[itemBarIndex].Count - 1];
		}
		return null;
    }

	public void Start(){
		for (int i = 0; i < 6; i++) {
			items[i] = GameObject.Find(string.Concat("item",(i+1).ToString())).GetComponent<Image>();
			amountText[i] = GameObject.Find (string.Concat("item",(i+1).ToString())).GetComponentInChildren<Text>();
		}
	}

    public void setUI() {
		for(int i = 0; i < 6; i++){
			amountText[i].text = itemBar[i].Count.ToString();
			items[i].material = (Material)Resources.Load("Menu/grassBlockMat");
		}
    }

	private int checkForEntry(string typeString){
		int listNumber = -1;
		for(int i = 0; i < 6; i++){
			//This is the case if the Array is empty
			if((itemBar [i]).Count < 1){
				listNumber = i;
			}else if((itemBar [i]) [0].GetType() == typeof(Block)){
				if (((Block)(itemBar [i]) [0]).BlockType == typeString) {
					if ((itemBar [i]).Count < 64) {
						listNumber = i;
						break;
					}
				}
			}
		}
		return listNumber;
	}
}
