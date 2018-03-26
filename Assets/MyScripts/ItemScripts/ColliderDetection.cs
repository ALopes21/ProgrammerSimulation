using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetection : MonoBehaviour {

    List<GameObject> currentCollisions = new List<GameObject>();
    MovableObject moveScript;

    // Use this for initialization
    void Start () {
        moveScript = gameObject.GetComponentInParent<MovableObject>();
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
            moveScript.moving = false;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Codeable")
        {
            currentCollisions.Remove(collision.gameObject);
            Debug.Log(collision + " Removed");
            StartCoroutine(WaitForSeconds(1));
            if (currentCollisions.Count <= 0)
            {
                //if (moveScript.handler.FloatInCell)
                //{
                    moveScript.moving = true;
                //}
            }
        }
    }

    IEnumerator WaitForSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
