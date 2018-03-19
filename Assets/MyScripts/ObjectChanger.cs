using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChanger : MonoBehaviour {

    public Sprite Disabled, Enabled;
    public GameObject newTarget, prevTarget;
    public GameObject newObject, prevObject;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

    public void ChangeTarget()
    {
        EnemyObject enemy = gameObject.GetComponent<EnemyObject>();
        prevTarget = enemy.target;
        enemy.target = newTarget;
    }

    public void ChangeSprite()
    {
        if(gameObject.name == "Enemy")
        {
            EnemyObject enemy = gameObject.GetComponent<EnemyObject>();
            prevObject = enemy.ammo;
            enemy.ammo = newObject;
        }
    }
}
