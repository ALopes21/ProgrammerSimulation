using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Every item's cell must contain this script
/// </summary>
[RequireComponent(typeof(Image))]
public class VariableSlot : MonoBehaviour, IDropHandler
{
    public enum VariableType
    {
        Float,
        Bool,
        GameObject,
        Sprite,
        Any,
        None
    }

    public VariableType variableType = VariableType.Float;
    public bool isTaken;
    public CodeHandler handler;
    public SceneHandler sceneHandler;

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
        //SetBackgroundState(GetComponentInChildren<DragAndDropItem>() == null ? false : true);
        SetBackgroundState(variableType);
        sceneHandler = GameObject.Find("Main Camera").GetComponent<SceneHandler>();
    }

    /// <summary>
    /// On any item drag start need to disable all items raycast for correct drop operation
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragStart(DragAndDropItem item)
    {
        DragAndDropItem myItem = GetComponentInChildren<DragAndDropItem>(); // Get item from current cell
        if (myItem != null)
        {
            myItem.MakeRaycast(false);                                      // Disable item's raycast for correct drop handling
            if (myItem == item)                                             // If item dragged from this cell
            {
                item.MakeVisible(false);                            // Hide item in cell till dragging
                isTaken = false;
            }
        }
    }

    /// <summary>
    /// On any item drag end enable all items raycast
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragEnd(DragAndDropItem item)
    {
        DragAndDropItem myItem = GetComponentInChildren<DragAndDropItem>(); // Get item from current cell
        if (myItem != null)
        {
            if (myItem == item)
            {
                isTaken = true;
            }
            myItem.MakeRaycast(true);                                       // Enable item's raycast
        }
        else
        {
            isTaken = false;
        }
    }

    /// <summary>
    /// Item is dropped in this cell
    /// </summary>
    /// <param name="data"></param>
    public void OnDrop(PointerEventData data)
    {
        handler = GameObject.Find("Main Camera").GetComponent<ObjectSelection>().activePanel.GetComponent<CodeHandler>();

        if (DragAndDropItem.icon != null)
        {
            if (DragAndDropItem.icon.activeSelf == true)                    // If icon inactive do not need to drop item in cell
            {
                DragAndDropItem item = DragAndDropItem.draggedItem;
                VariableSlot sourceCell = DragAndDropItem.sourceCell;
                if ((item != null) && (sourceCell != this) && (isTaken == false))
                {
                    Debug.Log("Passed all checks: " + isTaken);
                    switch (variableType)                                       // Check this cell's type
                    {
                        case VariableType.Float:
                            switch (item.itemVariableType)
                            {
                                case DragAndDropItem.ItemVariableType.Float:
                                    SetItem(item, sourceCell);
                                    handler.droppedFloat = item.float_prop;
                                    handler.droppedChar = item.char_prop;
                                    handler.DoTheMoveThing();
                                    break;
                                default:
                                    Debug.Log("ITEM NOT A FLOAT");
                                    sceneHandler.lives--;
                                    break;
                            }
                            break;
                        case VariableType.Bool:
                            switch (item.itemVariableType)
                            {
                                case DragAndDropItem.ItemVariableType.Bool:
                                    SetItem(item, sourceCell);
                                    handler.droppedBool = item.bool_prop;
                                    handler.DoTheColliderThing();
                                    break;
                                default:
                                    Debug.Log("ITEM NOT A BOOL");
                                    sceneHandler.lives--;
                                    break;
                            }
                            break;
                        case VariableType.GameObject:
                            switch (item.itemVariableType)
                            {
                                case DragAndDropItem.ItemVariableType.GameObject:
                                    SetItem(item, sourceCell);
                                    handler.droppedGO = item.GO_prop;
                                    handler.DoTheObjectThing();
                                    break;
                                default:
                                    Debug.Log("ITEM NOT A GAMEOBJECT");
                                    sceneHandler.lives--;
                                    break;
                            }
                            break;
                        case VariableType.Sprite:
                            switch (item.itemVariableType)
                            {
                                case DragAndDropItem.ItemVariableType.Sprite:
                                    SetItem(item, sourceCell);
                                    handler.droppedSprite = item.sprite_prop;
                                    handler.DoTheObjectThing();
                                    break;
                                default:
                                    Debug.Log("ITEM NOT A SPRITE");
                                    sceneHandler.lives--;
                                    break;
                            }
                            break;
                        case VariableType.Any:
                            switch(item.itemVariableType)
                            {
                                case DragAndDropItem.ItemVariableType.Float:
                                    SetItem(item, sourceCell);
                                    handler.droppedFloat = item.float_prop;
                                    handler.droppedChar = item.char_prop;
                                    handler.DoTheMoveThing();
                                    break;
                                case DragAndDropItem.ItemVariableType.Bool:
                                    SetItem(item, sourceCell);
                                    handler.droppedBool = item.bool_prop;
                                    handler.DoTheColliderThing();
                                    break;
                                case DragAndDropItem.ItemVariableType.GameObject:
                                    SetItem(item, sourceCell);
                                    handler.droppedGO = item.GO_prop;
                                    handler.DoTheObjectThing();
                                    break;
                                case DragAndDropItem.ItemVariableType.Sprite:
                                    SetItem(item, sourceCell);
                                    handler.droppedSprite = item.sprite_prop;
                                    handler.DoTheObjectThing();
                                    break;
                                default:
                                    Debug.Log("An error occurred");
                                    break;
                            }
                            break;
                        case VariableType.None:
                            SetItem(item, sourceCell);
                            handler.BackPeddle(item);
                            break;
                        default:
                            break;
                    }
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
    public void SetBackgroundState(VariableType type)
    {
        switch (type)
        {
            case VariableType.Bool:
                GetComponent<Image>().color = Color.red;
                break;
            case VariableType.Float:
                GetComponent<Image>().color = Color.blue;
                break;
            case VariableType.GameObject:
                GetComponent<Image>().color = Color.green;
                break;
            case VariableType.Sprite:
                GetComponent<Image>().color = Color.yellow;
                break;
            case VariableType.None:
                GetComponent<Image>().color = Color.grey;
                break;
            case VariableType.Any:
                GetComponent<Image>().color = Color.white;
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
                // Place first item into second cell
                firstItem.transform.SetParent(secondCell.transform, false);
                firstItem.transform.localPosition = Vector3.zero;
                secondCell.isTaken = true;
            }
            if (secondItem != null)
            {
                // Place second item into first cell
                secondItem.transform.SetParent(firstCell.transform, false);
                secondItem.transform.localPosition = Vector3.zero;
                firstCell.isTaken = true;
            }
        }
    }
}
