using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {

    public Vector2 myDirection;
    public SceneHandler handler;

	// Use this for initialization
	void Start () {
        handler = GameObject.Find("Finishline").GetComponent<SceneHandler>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), myDirection, 10 * Time.deltaTime);
        if (transform.position.x == myDirection.x && transform.position.y == myDirection.y)
        {
            Destroy(gameObject);
        }
    }

    public void MoveMe(Vector2 direction)
    {
        myDirection = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Destroy(gameObject);
                handler.lives--;
                break;
            case "breakable":
                GameObject.Find("BreakableWall").GetComponent<BreakableWall>().health--;
                break;
            case "Codeable":
                //Do nothing 
                break;
            default:
                Destroy(gameObject);
                break;

        }
    }
}
