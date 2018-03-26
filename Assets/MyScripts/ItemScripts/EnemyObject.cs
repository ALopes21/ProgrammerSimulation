using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour {

    public GameObject target;
    public GameObject ammo;
    private float shootCooldown;
    public float shootingRate = 3f;
    SceneHandler handler;

    // Use this for initialization
    void Start () {
        shootCooldown = 0f;
        handler = GameObject.Find("Main Camera").GetComponent<SceneHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!handler.gameOver)
        {
            if (shootCooldown > 0)
            {
                shootCooldown -= Time.deltaTime;
            }

            if (shootCooldown <= 0)
            {
                if (target != null)
                {
                    shootCooldown = shootingRate;
                    GameObject clone = Instantiate(ammo) as GameObject;
                    clone.transform.position = transform.position;
                    Vector2 direction = target.transform.position;
                    clone.GetComponent<Ammo>().MoveMe(direction);

                }
            }
        }
    }
}
