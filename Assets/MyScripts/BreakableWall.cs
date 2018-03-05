using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour {

    public int health;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        switch (health)
        {
            case 2:
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 0:
                Destroy(gameObject);
                break;
            default:
                break;
        }
	}

}
