﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Every item's cell must contain this script
/// </summary>
[RequireComponent(typeof(Image))]
public class VariableSlot : MonoBehaviour, IDropHandler
{

    public enum SlotType
    {
        Basic,
        Conditional,
        Looper
    }

    public enum ConditionType
    {
        If,
        This,
        Then,
        That,
        LoopInt,
        None
    }

    public SlotType thisSlotType = SlotType.Basic;
    public ConditionType thisConditionType = ConditionType.If;
    public VariableType.Type slotVariableType = VariableType.Type.Vector2;
    public bool isTaken;
    VariableValueHandler valueHandler;
    ObjectSelection ObjInt;

    public List<GameObject> LayeredSlots = new List<GameObject>();

    public string OriginalText;
    public Color OriginalColor;

    public struct DropDescriptor                                            // Struct with info about item's drop event
    {
        public VariableSlot sourceCell;                                  // From this cell item was dragged
        public VariableSlot destinationCell;                             // Into this cell item was dropped
        public DragAndDropItem item;                                        // dropped item
    }

    void OnEnable()
    {
        DragAndDropItem.OnItemDragStartEvent += OnAnyItemDragStart;         // Handle any item drag start
        DragAndDropItem.OnItemDragEndEvent += OnAnyItemDragEnd;             // Handle any item drag end
    }

    void OnDisable()
    {
        DragAndDropItem.OnItemDragStartEvent -= OnAnyItemDragStart;
        DragAndDropItem.OnItemDragEndEvent -= OnAnyItemDragEnd;
    }

    void Start()
    {
        if(!gameObject.tag.Contains("Slot"))
        {
            OriginalText = gameObject.GetComponentInChildren<Text>().text;
            OriginalColor = gameObject.GetComponentInChildren<Text>().color;
        }

        if (slotVariableType == VariableType.Type.Layered)
        {
            Transform panel = transform.GetChild(1);
            foreach (Transform child in panel)
            {
                if (child.gameObject.tag.Contains("LayeredSlots"))
                {
                    if (!LayeredSlots.Contains(child.gameObject))
                    { LayeredSlots.Add(child.gameObject); }

                }
            }
        }

        //SetBackgroundState(GetComponentInChildren<DragAndDropItem>() == null ? false : true);
        SetBackgroundState(slotVariableType);
        valueHandler = GameObject.Find("Main Camera").GetComponent<VariableValueHandler>();
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectSelection>();
        if (thisConditionType == ConditionType.If)
        {
           GetComponent<Image>().sprite = (Sprite)Resources.Load("SlotSprites/" + ObjInt.currentObject.ToString(), typeof(Sprite));
           GetComponent<VariableSlot>().enabled = false; 
        }
    }

    public void ResetSlot()
    {
        gameObject.GetComponentInChildren<Text>().text = OriginalText;
        gameObject.GetComponentInChildren<Text>().color = OriginalColor;
    }

    /// <summary>
    /// On any item drag start need to disable all items raycast for correct drop operation
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragStart(DragAndDropItem item)
    {
        DragAndDropItem myItem = GetComponentInChildren<DragAndDropItem>(); // Get item from current cell
        if (myItem != null){
            myItem.MakeRaycast(false);                                      // Disable item's raycast for correct drop handling
            if (myItem == item){                                             // If item dragged from this cell
                item.MakeVisible(false);                            // Hide item in cell till dragging
                isTaken = false;}}
    }

    /// <summary>
    /// On any item drag end enable all items raycast
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragEnd(DragAndDropItem item)
    {
        DragAndDropItem myItem = GetComponentInChildren<DragAndDropItem>(); // Get item from current cell
        if (myItem != null){
            if (myItem == item){isTaken = true;}
            myItem.MakeRaycast(true);                                       // Enable item's raycast
        }else{isTaken = false;}
    }

    /// <summary>
    /// Item is dropped in this cell
    /// </summary>
    /// <param name="data"></param>
    public void OnDrop(PointerEventData data)
    {
        if (DragAndDropItem.icon != null)
        {
            if (DragAndDropItem.icon.activeSelf == true)                    // If icon inactive do not need to drop item in cell
            {
                DragAndDropItem item = DragAndDropItem.draggedItem;
                VariableSlot sourceCell = DragAndDropItem.sourceCell;
                if ((item != null) && (sourceCell != this) && (isTaken == false))
                {
                    Debug.Log("Passed all checks...");
                    valueHandler.CheckSlotVariableType(item, this);   
                }
                if (item.GetComponentInParent<VariableSlot>() == null)   // If item have no cell after drop
                {
                    Destroy(item.gameObject);                               // Destroy it
                    Debug.Log("No cell: Item was destroyed");
                }
            }
        }
    }

    public void SetItem(DragAndDropItem item, VariableSlot sourceCell)
    {
        DragAndDropItem currentItem = GetComponentInChildren<DragAndDropItem>();
        SwapItems(sourceCell, this);            // Swap items between cells
    }

    /// <summary>
    /// Change cell's sprite color on item put/remove
    /// </summary>
    /// <param name="condition"> true - filled, false - empty </param>
    public void SetBackgroundState(VariableType.Type type)//VariableType type)
    {
        switch (type)
        {
            case VariableType.Type.Bool:
                //GetComponentInChildren<Text>().color = Color.red;
                break;
            case VariableType.Type.Vector2:
                GetComponentInChildren<Text>().color = Color.blue;
                break;
            case VariableType.Type.GameObject:
                GetComponentInChildren<Text>().color = Color.magenta;
                break;
            case VariableType.Type.Sprite:
                GetComponentInChildren<Text>().color = Color.yellow;
                break;
            case VariableType.Type.Int:
                GetComponentInChildren<Text>().color = Color.black;
                break;
            case VariableType.Type.Layered:
                GetComponentInChildren<Text>().color = Color.green;
                break;
            case VariableType.Type.None:
                GetComponent<Image>().color = Color.grey; //Use for all Inventory Slots!
                if (thisConditionType == ConditionType.If)
                {
                    GetComponent<Image>().color = Color.clear;
                }
                break;
            case VariableType.Type.Any:
                GetComponentInChildren<Text>().color = Color.white;
                break;
            default:
                Debug.Log("An error occurred");
                break;

        }
        //GetComponent<Image>().color = type ? full : empty;
    }

    /// <summary>
    /// Delete item from this cell
    /// </summary>
    public void RemoveItem()
    {
        foreach (DragAndDropItem item in GetComponentsInChildren<DragAndDropItem>())
        {
            Destroy(item.gameObject);
        }
        isTaken = false;
    }

    /// <summary>
    /// Put new item in this cell
    /// </summary>
    /// <param name="itemObj"> New item's object with DragAndDropItem script </param>
    public void PlaceItem(GameObject itemObj)
    {
        //RemoveItem();                                                       // Remove current item from this cell
        if (itemObj != null)
        {
            GameObject newItem = Instantiate(itemObj, transform);
            newItem.transform.localPosition = Vector3.zero;
            DragAndDropItem item = newItem.GetComponent<DragAndDropItem>();
            if (item != null)
            {
                item.MakeRaycast(true);
            }
            isTaken = true;
        }
    }

    /// <summary>
    /// Get item from this cell
    /// </summary>
    /// <returns> Item </returns>
    public DragAndDropItem GetItem()
    {
        return GetComponentInChildren<DragAndDropItem>();
    }

    /// <summary>
    /// Swap items between to cells
    /// </summary>
    /// <param name="firstCell"> Cell </param>
    /// <param name="secondCell"> Cell </param>
    public void SwapItems(VariableSlot firstCell, VariableSlot secondCell)
    {
        if ((firstCell != null) && (secondCell != null))
        {
            DragAndDropItem firstItem = firstCell.GetItem();                // Get item from first cell
            DragAndDropItem secondItem = secondCell.GetItem();              // Get item from second cell
            if (firstItem != null)
            {
                Debug.Log("Setting first item " + firstItem.name);
                // Place first item into second cell
                firstItem.transform.SetParent(secondCell.transform, false);
                firstItem.transform.localPosition = Vector3.zero;
                secondCell.isTaken = true;
            }
            if (secondItem != null)
            {
                Debug.Log("Setting second item " + secondItem.name);
                // Place second item into first cell
                secondItem.transform.SetParent(firstCell.transform, false);
                secondItem.transform.localPosition = Vector3.zero;
                firstCell.isTaken = true;
            }
        }
    }

}
