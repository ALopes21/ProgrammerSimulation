using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{

    private GameObject CodePanel;
    public GameObject currentObject;
    //private GameObject[] cells;

    // Use this for initialization
    void Start()
    {
        CodePanel = GameObject.FindGameObjectWithTag("CodePanel");
        //cells = GameObject.FindGameObjectsWithTag("Cell");
        //for (int i = 0; i < cells.Length; i++)
        //{
        //    cells[i].SetActive(false);
        //}
        CodePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                if (hit != null && hit.collider != null)
                {
                    Debug.Log("I'm hitting " + hit.collider.name);
                    if (hit.collider.tag == "Codeable")
                    {
                        currentObject = hit.collider.gameObject;
                        SetupCodePanel(currentObject);
                    }
                }
            }
        }
    }

    //Change this to activate gameObject/sprite instead of text
    public void SetupCodePanel(GameObject selectedObject)
    {
        CodePanel.SetActive(true);
        ObjectData data = selectedObject.GetComponent<ObjectData>();
        string codeData = data.codedata.text;
        CodePanel.GetComponentInChildren<Text>().text = codeData;
        //CheckBooleans(data);
    }
    //public void ActivateCell(VariableSlot.VariableType type)
    //{
    //    for (int j = 0; j < cells.Length; j++)
    //    {
    //        if (cells[j].activeInHierarchy == false)
    //        {
    //            cells[j].SetActive(true);
    //            cells[j].GetComponent<VariableSlot>().variableType = type;
    //            cells[j].GetComponent<VariableSlot>().SetBackgroundState(type);
    //            break;
    //        }
    //    }
    //}

    //public void CheckBooleans(ObjectData data)
    //{
    //    if (data.Move)
    //    {
    //        ActivateCell(VariableSlot.VariableType.Char);
    //        ActivateCell(VariableSlot.VariableType.Float);
    //    }
    //    if (data.Collider)
    //    {
    //        ActivateCell(VariableSlot.VariableType.Bool);
    //    }
    //}

    public void DeselectCurrentObject()
    {
        currentObject = null;
        //for (int i = 0; i < cells.Length; i++)
        //{
        //    cells[i].SetActive(false);
        //}
        CodePanel.SetActive(false);
    }
}
