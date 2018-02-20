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
        CodePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1 || Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))//Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
    }

    public void DeselectCurrentObject()
    {
        currentObject = null;
        CodePanel.SetActive(false);
    }
}
