using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChanger : MonoBehaviour {

    public Sprite Disabled, Enabled;
    public bool originalBool;
    public GameObject newTarget, originalTarget;
    public GameObject newObject, originalObject;

    public void ToggleCollider(bool value)
    {
        Collider2D col = gameObject.GetComponent<Collider2D>();
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        switch (value)
        {
            case true:
                col.enabled = true;
                renderer.sprite = Enabled;
                break;
            case false:
                col.enabled = false;
                renderer.sprite = Disabled;
                break;
            default:
                Debug.Log("Switch Case Error: check collider2D");
                break;
        }
    }

    public void ChangeTarget()
    {
        EnemyObject enemy = gameObject.GetComponent<EnemyObject>();
        enemy.target = newTarget;
    }

    public void ChangeSprite()
    {
        if(gameObject.name == "Enemy")
        {
            EnemyObject enemy = gameObject.GetComponent<EnemyObject>();
            enemy.ammo = newObject;
        }
    }
}
