using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container {
    private List<Item>[] container;
    private Text[] slotItemCounts;
    private Image[] slotItemImages;
    private int containerSize;
	private GameObject selfRef;
	public string name;

    public readonly static int SLOT_SIZE = 64;

	/// <summary>
	/// Initializes a new instance of the Container class.
	/// </summary>
	/// <param name="containerSize">Container size.</param>
	/// <param name="name">Name.</param>
    public Container(int containerSize, string name) {
        this.containerSize = containerSize;
		this.name = name;

        container = new List<Item>[containerSize];
        slotItemCounts = new Text[containerSize];
        slotItemImages = new Image[containerSize];
		selfRef = GameObject.Find(name);

        slotItemCounts = selfRef.GetComponentsInChildren<Text>();
        slotItemImages = selfRef.GetComponentsInChildren<Image>();

        for (int i = 0; i < containerSize; i++) {
            container[i] = new List<Item>(64);
			slotItemImages[i].gameObject.AddComponent<ItemDragHandler>();
        }
		selfRef.AddComponent<ItemDropHandler>();
    }

	/// <summary>
	/// Tests to see if the container is full.
	/// </summary>
	/// <returns><c>true</c>, if full, <c>false</c> otherwise.</returns>
    public bool isFull() {
        foreach (List<Item> list in container) {
            if (list.Count < SLOT_SIZE)
                return false;
        }
        return true;
    }

	/// <summary>
	/// Adds the item.
	/// </summary>
	/// <returns><c>true</c>, if item was added, <c>false</c> otherwise.</returns>
	/// <param name="item">Item.</param>
    public bool addItem(Item item) {
        int currentIndex = checkIfListFree(item.resourceString);
        if (currentIndex != -1) {
            container[currentIndex].Add(item);
			ObjectiveManager.Instance.ObjectiveCheck ("inventory", item.resourceString, container[currentIndex].Count);
            return true;
        }
        return false;
    }

	/// <summary>
	/// Gets the item.
	/// </summary>
	/// <returns>The item if it exists in the given slot, <c>null</c> otherwise.</returns>
	/// <param name="slot">Slot.</param>
	public Item getItem(int slot){
		if (container[slot].Count > 0) {
			return container [slot] [0];
		}
		return null;
	}

	/// <summary>
	/// Removes the item.
	/// </summary>
	/// <returns>The type of item as a string.</returns>
	/// <param name="slot">Slot.</param>
    public string removeItem(int slot) {
        int currentSlotSize = container[slot].Count;
        List<Item> currentSlot = container[slot];

        if (currentSlotSize > 0) {
            string typeOfBlock = currentSlot[currentSlotSize - 1].resourceString;
            currentSlot.RemoveAt(currentSlotSize - 1);
            return typeOfBlock;
        }
        return null;
    }

	/// <summary>
	/// Sets the UI element associated with this container.
	/// </summary>
    public void setUI() {
        string materialName;
        int currentSlotSize = 0;

        for (int i = 0; i < containerSize; i++) {
            currentSlotSize = container[i].Count;
            materialName = (currentSlotSize > 0) ? (container[i])[0].resourceString : "DefaultItem";

            slotItemCounts[i].text = currentSlotSize.ToString();
            slotItemImages[i].material = (Material)Resources.Load(string.Concat("Menu/", materialName));
        }
    }

	/// <summary>
	/// Gets the boundary.
	/// </summary>
	/// <returns>The boundary.</returns>
	public RectTransform getBoundary(){
		return selfRef.transform as RectTransform;
	}

	/// <summary>
	/// Finds the slot that isnt equal to the second parameter.
	/// </summary>
	/// <returns>The slot.</returns>
	/// <param name="mousePosition">Mouse position.</param>
	/// <param name="oldContainerIndex">Old container index.</param>
	public int findSlotThatIsnt(Vector3 mousePosition, int oldContainerIndex){
		foreach(Image img in slotItemImages){
			if (RectTransformUtility.RectangleContainsScreenPoint(img.rectTransform, mousePosition)) {
				if(img.transform.GetSiblingIndex() != oldContainerIndex){
					return img.transform.GetSiblingIndex();
				}
			}
		}
		return -1;
	}

	/// <summary>
	/// Adds items at the index of the third parameter.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="numOfItems">Number of items.</param>
	/// <param name="index">Index.</param>
	public void addItemsAtIndex(Item item, int numOfItems, int index){
		int i = 0;
		if (container [index].Count < 1) {
			for (i = 0; i < numOfItems; i++) {
				container [index].Add (item);
			}
		} else if (container [index] [0].resourceString == item.resourceString) {
			while (container [index].Count <= 64 && i < numOfItems) {
				i++;
				container [index].Add (item);
			}
		}
	}

	/// <summary>
	/// Removes all items at the given index.
	/// </summary>
	/// <param name="index">Index.</param>
	public void removeItemsAtIndex(int index){
		while(container[index].Count > 0){
			container[index].RemoveAt(container[index].Count-1);
		}
	}

	/// <summary>
	/// Toggle this instance.
	/// </summary>
	public bool toggle(){
		selfRef.SetActive(!selfRef.activeSelf);
		return selfRef.activeSelf;
	}

	/// <summary>
	/// Gets the size of the container.
	/// </summary>
	/// <returns>The container size.</returns>
	public int getContainerSize() {
		return container.Length;
	}

	/// <summary>
	/// Checks if there is a free list to put items in.
	/// </summary>
	/// <returns>The index of a free list.</returns>
	/// <param name="itemName">Item name.</param>
    private int checkIfListFree(string itemName) {
        int index = -1;
        int currentSlotSize = 0;
        for (int i = 0; i < containerSize; i++) {
            currentSlotSize = container[i].Count;
            if (currentSlotSize == 0) {
                index = i;
            } else if (currentSlotSize < SLOT_SIZE) {
                if ((container[i])[0].resourceString == itemName) {
                    index = i;
                    break;
                }
            }

        }
        return index;
    }
}