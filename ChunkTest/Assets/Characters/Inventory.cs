using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    private Stack<Block> blocks = new Stack<Block>();
	public Text blockAmountText;
	public Image item1;

    public void addBlock(Block block){
        blocks.Push(block);
    }
    public Block getBlock(){
        return blocks.Pop();
    }

	public void Start(){
		item1 = GameObject.Find ("item1").GetComponent<Image>();
		blockAmountText = GameObject.Find ("item1").GetComponentInChildren<Text>();
	}

    public void setUI() {
		Debug.Log (blocks.Count.ToString());
		blockAmountText.text = blocks.Count.ToString();
		item1.material = (Material)Resources.Load("Menu/grassBlockMat");
    }
}
