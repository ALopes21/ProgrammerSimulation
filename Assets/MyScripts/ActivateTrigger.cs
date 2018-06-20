using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityStandardAssets.Utility
{
    public class ActivateTrigger : MonoBehaviour
    {
        // A multi-purpose script which causes an action to occur when
        // a trigger collider is entered.
        public enum Mode
        {
            Trigger = 0,    // Just broadcast the action on to the target
            Replace = 1,    // replace target with source
            Activate = 2,   // Activate the target GameObject
            Enable = 3,     // Enable a component
            Animate = 4,    // Start animation on target
            Deactivate = 5,  // Decativate target GameObject
            GetVariables = 6,    //Put variables into slots and destroy chest
            LoadScene = 7 //load next scene
        }

        public Mode action = Mode.Activate;         // The action to accomplish
        public Object target;                       // The game object to affect. If none, the trigger work on this game object
        public GameObject source;
        public int triggerCount = 1;
        public bool repeatTrigger = false;


        public SceneHandler handler;


        private void Start()
        {
            handler = GameObject.Find("Main Camera").GetComponent<SceneHandler>();
        }

        private void DoActivateTrigger(Collider2D col)
        {
            triggerCount--;

            if (triggerCount == 0 || repeatTrigger)
            {
                Object currentTarget = target ?? gameObject;
                Behaviour targetBehaviour = currentTarget as Behaviour;
                GameObject targetGameObject = currentTarget as GameObject;
                if (targetBehaviour != null)
                {
                    targetGameObject = targetBehaviour.gameObject;
                }

                switch (action)
                {
                    case Mode.Trigger:
                        if (targetGameObject != null)
                        {
                            targetGameObject.BroadcastMessage("DoActivateTrigger");                          
                        }
                        break;
                    case Mode.Replace:
                        if (source != null)
                        {
                            if (targetGameObject != null)
                            {
                                Instantiate(source, targetGameObject.transform.position,
                                            targetGameObject.transform.rotation);
                                Destroy(targetGameObject);
                            }
                        }
                        break;
                    case Mode.Activate:
                        
                        break;
                    case Mode.Enable:
                        if (targetBehaviour != null)
                        {
                            targetBehaviour.enabled = true;
                        }
                        break;
                    case Mode.Animate:
                        if (targetGameObject != null)
                        {
                            targetGameObject.GetComponent<Animation>().Play();
                        }
                        break;
                    case Mode.Deactivate:
                        if (targetGameObject != null)
                        {
                            targetGameObject.SetActive(false);
                        }
                        break;
                    case Mode.GetVariables:
                        for (int i = 0; i < handler.Items.Length; i++)
                            {
                                for(int j = 0; j < handler.InvSlots.Length; j++)
                                {
                                    if (handler.InvSlots[j].GetComponent<VariableSlot>().isTaken == false)
                                    {
                                        handler.InvSlots[j].GetComponent<VariableSlot>().PlaceItem(handler.Items[i]);
                                        Destroy(handler.Items[i]);
                                        break;
                                        
                                    }
                                }
                            }
                        break;
                    case Mode.LoadScene:
                        if (col.tag == "Player")
                        {
                            switch (this.gameObject.name)
                            {
                                case "Finishline":
                                    handler.LoadScene(0);
                                    handler.UpdateLevelInfo();
                                    break;
                                case "Killzone":
                                    targetGameObject.SetActive(true);
                                    handler.gameOver = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                }
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            DoActivateTrigger(other);
        }
    }
}
