using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    private Stack<Block> blocks = new Stack<Block>();
    public Text BlockAmountText;
    public void addBlock(Block block){
        blocks.Push(block);
    }
    public Block getBlock(){
        return blocks.Pop();
    }

    public void setUI() {
        BlockAmountText.text = "Blocks: " + blocks.Count;
    }
}
