using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {

    public Sprite originalSprite;
    public Vector2 myDirection;
    public SceneHandler scene_handler;

	// Use this for initialization
	void Start () {
        scene_handler = GameObject.Find("Finishline").GetComponent<SceneHandler>();
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
        if(gameObject.tag == "SoftAmmo")
        {
            switch (collision.gameObject.tag)
            {
                case "Player":
                    Destroy(gameObject);
                    if(scene_handler.lives < 3)
                    {
                        scene_handler.lives++;
                    }
                    break;
                case "Codeable":
                    //Do nothing 
                    break;
                default:
                    Destroy(gameObject);
                    break;

            }
        }
        if(gameObject.tag == "HardAmmo")
        {
            switch (collision.gameObject.tag)
            {
                case "Player":
                    Destroy(gameObject);
                    scene_handler.lives--;
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
}
