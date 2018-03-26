using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {

    public Vector2 newPos;
    public bool moving;
    public Vector2 previousPos;
    ObjectSelection ObjInt;
    public CodeHandler handler;

    public Collider2D[] detectionCols;

    // Use this for initialization
    void Start ()
    {
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectSelection>();
        detectionCols = gameObject.GetComponentsInChildren<Collider2D>();
        foreach(Collider2D col in detectionCols)
        {
            if(col.transform.childCount > 0)
            {
                Debug.Log(col.gameObject.name + " is the parent");
            }
            else
            {
                col.enabled = false;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (moving)
        {
            if (newPos != new Vector2(0, 0))
            {
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), newPos, 3 * Time.deltaTime);
                if (transform.position.x == newPos.x && transform.position.y == newPos.y)
                {
                    moving = false;
                }
            }
        }
    }

    public void MoveObject(float variable, char axis)
    {
        handler = ObjInt.activePanel.GetComponent<CodeHandler>();
        previousPos = gameObject.transform.position;
        newPos = gameObject.transform.position;

        switch (axis)
        {
            case 'x':
                newPos.x += variable;
                moving = true;
                if(variable > 0)
                {
                    foreach (Collider2D col in detectionCols)
                    {
                        if(col.gameObject.name == "rightCol")
                        {
                            col.enabled = true;
                        }
                    }
                }
                if(variable < 0)
                {
                    foreach (Collider2D col in detectionCols)
                    {
                        if (col.gameObject.name == "leftCol")
                        {
                            col.enabled = true;
                        }
                    }
                }
                break;
            case 'y':
                newPos.y += variable;
                moving = true;
                if (variable > 0)
                {
                    foreach (Collider2D col in detectionCols)
                    {
                        if (col.gameObject.name == "topCol")
                        {
                            col.enabled = true;
                        }
                    }
                }
                if (variable < 0)
                {
                    foreach (Collider2D col in detectionCols)
                    {
                        if (col.gameObject.name == "bottomCol")
                        {
                            col.enabled = true;
                        }
                    }
                }
                break;
            default:
                Debug.Log("Switch Case Error: undefined char");
                break;
        }
    }

    public void MoveBack()
    {
        gameObject.transform.position = previousPos;
        newPos = previousPos;
        foreach (Collider2D col in detectionCols)
        {
            if (col.transform.childCount > 0)
            {
                Debug.Log(col.gameObject.name + " is the parent");
            }
            else
            {
                col.enabled = false;
            }
        }
    }
}
