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
        Star = (Sprite)Resources.Load("UISprites/Star", typeof(Sprite));
        NoStar = (Sprite)Resources.Load("UISprites/NoStar", typeof(Sprite));
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

    public void SetupMenuItems()
    {
        int listCount = Persistence.savedLevels.Count;
        int highestActiveLevel = Persistence.savedLevels[listCount - 1].currentLevel;
        levelButtons[highestActiveLevel].GetComponent<Button>().interactable = true;

        for (int i =0; i < Persistence.savedLevels.Count; i++)
        {
            Debug.Log("Saved Level: " + Persistence.savedLevels[i].Stars);

            levelButtons[i].transform.GetChild(3).GetComponent<Text>().text = Persistence.savedLevels[i].Number.ToString();
            levelButtons[i].transform.GetChild(4).GetComponent<Text>().text = Mathf.RoundToInt(Persistence.savedLevels[i].Time).ToString() + "s";

            int levelIndex = Persistence.savedLevels[i].Number;
            levelButtons[levelIndex - 1].GetComponent<Button>().interactable = true;

            for(int s = 0; s < Persistence.savedLevels[i].Stars; s++)
            {
                levelButtons[i].transform.GetChild(s).GetComponent<Image>().sprite = Star;
            }

        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void GetStarted()
    {
        GameObject.Find("LevelOne").GetComponent<Button>().interactable = true;
        Destroy(GameObject.Find("GetStartedButton"));
    }
}