using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    private Stack<Block> blocks = new Stack<Block>();
	public Text blockAmountText;

    public void addBlock(Block block){
        blocks.Push(block);
    }
    public Block getBlock(){
        return blocks.Pop();
    }

	public void Start(){
		blockAmountText = GameObject.Find("blockAmountText").GetComponent<Text>();
	}

    public void setUI() {
		Debug.Log (blocks.Count.ToString());
		blockAmountText.text = "Blocks: " + blocks.Count.ToString();
    }
}
