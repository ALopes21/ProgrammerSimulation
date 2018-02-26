using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour {

    public Sprite Disabled, Enabled;
    public TextAsset codedata;
    public Vector2 previousPos;
    public bool moving;

    public bool FloatinCell, BoolinCell;

    //Variables that the gameObject needs
    //public bool Move, Collider, Sprite;
    
    private Vector2 newPos;

    // Use this for initialization
    void Start () {
        codedata = (TextAsset)Resources.Load(gameObject.name + "Code", typeof(TextAsset));
    }

    // Update is called once per frame
    void Update ()
    {
        if (moving)
        {
            if(newPos != new Vector2(0,0))
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

    public void ToggleCollider()
    {
        Collider2D col = gameObject.GetComponent<Collider2D>();
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();

        switch (col.enabled)
        {
            case true:
                col.enabled = false;
                renderer.sprite = Disabled;
                break;
            case false:
                col.enabled = true;
                renderer.sprite = Enabled;
                break;
            default:
                Debug.Log("Switch Case Error: check collider2D");
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Codeable")
        {
            Debug.Log("Collided");
            moving = false;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Codeable")
        {
            StartCoroutine(WaitForSeconds(1));
            if (FloatinCell)
            {
                Debug.Log("Exited");
                moving = true;
            }
        }
    }

    IEnumerator WaitForSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
