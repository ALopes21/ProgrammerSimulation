using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour {

    public Sprite Disabled, Enabled;
    public TextAsset codedata;
    public Vector2 previousPos;
    public bool moving;

    //Variables that the gameObject needs
    public bool Move, Collider, Sprite;
    
    private Vector2 newPos;

    // Use this for initialization
    void Start () {

        Enabled = Resources.Load(gameObject.name + "ACol", typeof(Sprite)) as Sprite;
        Disabled = Resources.Load(gameObject.name + "DCol", typeof(Sprite)) as Sprite;
        codedata = (TextAsset)Resources.Load(gameObject.name + "Code", typeof(TextAsset));
        moving = false;
    }

    // Update is called once per frame
    void Update () {
        if(moving)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),newPos, 3 * Time.deltaTime);
            if(transform.position.x == newPos.x && transform.position.y == newPos.y)
            {
                moving = false;
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

        switch (col.isTrigger)
        {
            case true:
                col.isTrigger = false;
                renderer.sprite = Enabled;
                break;
            case false:
                col.isTrigger = true;
                renderer.sprite = Disabled;
                break;
            default:
                Debug.Log("Switch Case Error: check collider2D");
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Codeable")
        {
            Debug.Log("Triggered");
            moving = false;
        }
    }
}
