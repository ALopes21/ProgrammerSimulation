using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets._2D
{
    public class SceneHandler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                switch(this.gameObject.name)
                {
                    case "Killzone":
                        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
                        break;
                    case "Finishline":
                        //change this to menu scene.
                        SceneManager.LoadScene("LevelTwo");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
