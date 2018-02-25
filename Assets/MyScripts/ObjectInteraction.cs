using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{

    private GameObject[] CodePanel;
    public GameObject currentObject;
    public GameObject activePanel;

    // Use this for initialization
    void Start()
    {
        CodePanel = GameObject.FindGameObjectsWithTag("CodePanel");
        for (int i = 0; i < CodePanel.Length; i++)
        {
            CodePanel[i].SetActive(false);
        }
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
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 5f, 1 << LayerMask.NameToLayer("Objects"));
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
        for(int i = 0; i< CodePanel.Length; i++)
        {
            if(CodePanel[i].gameObject.name == selectedObject.name + "Code")
            {
                CodePanel[i].SetActive(true);
                activePanel = CodePanel[i];
                ObjectData data = selectedObject.GetComponent<ObjectData>();
                string codeData = data.codedata.text;
                activePanel.GetComponentInChildren<Text>().text = codeData;
                break;
            }
        }
    }

    public void DeselectCurrentObject()
    {
        for (int i = 0; i < CodePanel.Length; i++)
        {
            CodePanel[i].SetActive(false);
        }
        Collider2D col = currentObject.GetComponent<Collider2D>();
        currentObject = null;
        activePanel = null;
    }
}
