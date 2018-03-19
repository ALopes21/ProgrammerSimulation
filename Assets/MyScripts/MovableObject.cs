using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {

    public Vector2 newPos;
    public bool moving;
    public Vector2 previousPos;
    ObjectInteraction ObjInt;
    public CodeHandler handler;

    List<GameObject> currentCollisions = new List<GameObject>();

    // Use this for initialization
    void Start () {
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectInteraction>();
        //handler = ObjInt.activePanel.GetComponent<CodeHandler>();
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
                break;
            case 'y':
                newPos.y += variable;
                moving = true;
                break;
            default:
                Debug.Log("Switch Case Error: undefined char");
                break;
        }
    }

    public void MoveBack()
    {
        gameObject.transform.position = previousPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Note: probably dont need to check tag, we may want this for non codeable objects too
        if (collision.gameObject.tag == "Codeable")
        {
            currentCollisions.Add(collision.gameObject);
            foreach (GameObject gObject in currentCollisions)
            {
                Debug.Log(gObject + " Collided");
            }
                moving = false;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Codeable")
        {
            currentCollisions.Remove(collision.gameObject);
            Debug.Log(collision + " Removed");
            StartCoroutine(WaitForSeconds(1));
            if(currentCollisions.Count <= 0)
            {
                if (handler.FloatInCell)
                {
                    moving = true;
                }
            }
        }
    }

    IEnumerator WaitForSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
