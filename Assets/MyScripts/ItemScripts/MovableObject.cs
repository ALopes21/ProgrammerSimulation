using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MovableObject : MonoBehaviour {

    public Vector2 newPos;
    public bool moving, looping, looped;
    public Vector2 previousPos;
    ObjectSelection ObjInt;
    public CodeHandler handler;

    public Collider2D[] detectionCols;

    // Use this for initialization
    void Start ()
    {
        looping = false;
        looped = false;
        previousPos = gameObject.transform.position;
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
                    if (looped) { if (--GenericClass.loopCount > 0) { handler.DoTheLoopThing(); } looped = false; return; }
                    if (looping == true){ Invoke("MoveBack",1); looping = false; looped = true; }
                }
            }
        }
    }

    public void MoveObject(Vector2 variable)
    {
        handler = ObjInt.activePanel.GetComponent<CodeHandler>();
        previousPos = gameObject.transform.position;
        newPos = gameObject.transform.position;

        if(variable.x > 0)
        {
            newPos.x += variable.x;
            moving = true;
            foreach (Collider2D col in detectionCols)
            {
                if (col.gameObject.name == "rightCol")
                {
                    col.enabled = true;
                }
            }
        }
        else if(variable.x < 0)
        {
            newPos.x += variable.x;
            moving = true;
            foreach (Collider2D col in detectionCols)
            {
                if (col.gameObject.name == "leftCol")
                {
                    col.enabled = true;
                }
            }
        }

        if(variable.y > 0)
        {
            newPos.y += variable.y;
            moving = true;
            foreach (Collider2D col in detectionCols)
            {
                if (col.gameObject.name == "topCol")
                {
                    col.enabled = true;
                }
            }
        }
        else if(variable.y < 0)
        {
            newPos.y += variable.y;
            moving = true;
            foreach (Collider2D col in detectionCols)
            {
                if (col.gameObject.name == "bottomCol")
                {
                    col.enabled = true;
                }
            }
        }

    }

    public void MoveBack()
    {
        newPos = previousPos;
        moving = true;
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
