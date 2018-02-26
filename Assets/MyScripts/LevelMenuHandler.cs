using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelMenuHandler : MonoBehaviour
{

    public List<GameObject> levelButtons = new List<GameObject>();
    public Sprite Star, NoStar;

    private void Start()
    {
        Persistence.Load();
        Star = (Sprite)Resources.Load("Star", typeof(Sprite));
        NoStar = (Sprite)Resources.Load("NoStar", typeof(Sprite));
        for(int i = 0; i < gameObject.transform.childCount; i++ )
        {
            if (gameObject.transform.GetChild(i).tag == "LevelButton")
            {
                levelButtons.Add(gameObject.transform.GetChild(i).gameObject);
            }
        }
        for(int j = 0; j < levelButtons.Count; j++)
        {
            levelButtons[j].GetComponent<Button>().interactable = false;
        }

        SetupMenuItems();
    }
    private void FixedUpdate()
    {
    }

    public void SetupMenuItems()
    {

        for (int i =0; i < Persistence.savedLevels.Count; i++)
        {
            Debug.Log("Saved Level: " + Persistence.savedLevels[i].Number);

            levelButtons[i].GetComponentInChildren<Text>().text = Persistence.savedLevels[i].Number.ToString();

            for(int s = 0; s < Persistence.savedLevels[i].Stars; s++)
            {
                levelButtons[i].transform.GetChild(s).GetComponent<Image>().sprite = Star;
            }

            //Add Time Text
            switch (Persistence.savedLevels[i].Unclocked)
            {
                case true:
                    levelButtons[i].GetComponent<Button>().interactable = true;
                    break;
                case false:
                    levelButtons[i].GetComponent<Button>().interactable = false;
                    break;
            }
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}