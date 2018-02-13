using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour {

    public Sprite Disabled, Enabled;
    public TextAsset codedata;

    //Variables that the gameObject needs
    public bool Move, Collider, Sprite;

    // Use this for initialization
    void Start () {

        Enabled = Resources.Load(gameObject.name + "ACol", typeof(Sprite)) as Sprite;
        Disabled = Resources.Load(gameObject.name + "DCol", typeof(Sprite)) as Sprite;
        codedata = (TextAsset)Resources.Load(gameObject.name + "Code", typeof(TextAsset));
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void MoveObject(float variable, char axis)
    {
        Vector3 newPos = gameObject.transform.position;

        switch (axis)
        {
            case 'x':
                newPos.x += variable;
                gameObject.transform.position = newPos;
                break;
            case 'y':
                newPos.y += variable;
                gameObject.transform.position = newPos;
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
}
