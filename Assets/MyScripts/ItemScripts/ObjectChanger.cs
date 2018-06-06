using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChanger : MonoBehaviour {

    public Sprite Disabled, Enabled;
    public bool originalBool;
    public GameObject newTarget, originalTarget;
    public GameObject newObject, originalObject;

    public GameObject Cousin;

    public enum Conditions
    {
        BoolToBool,
        BoolToFloat,
        FloatToBool,
        FloatToFloat
    }

    public Conditions thisCondition = Conditions.BoolToBool;

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

    public void RunCondition(object condition, object thenObject, object action)
    {
        switch(thisCondition)
        {
            case Conditions.BoolToBool:
                Debug.Log(thenObject.GetType().Name.ToString());
                if(thenObject.GetType().Name == "GameObject")
                {
                    Debug.Log(condition.GetType().Name.ToString() + " : " + action.GetType().Name.ToString());
                    if (condition.GetType().Name == "Boolean" && action.GetType().Name == "Boolean")
                    {
                        if(GetComponent<Collider2D>().enabled == (bool)condition)
                        {
                            Cousin = (GameObject)thenObject;
                            Cousin.GetComponent<Collider2D>().enabled = (bool)action;
                            Debug.Log("Affect Cousin: " + Cousin + " : " + action);
                        }
                    }
                }
                break;
            case Conditions.BoolToFloat:
                break;
        }
    }
}
