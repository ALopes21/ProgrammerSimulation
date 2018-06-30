using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelMenuHandler : MonoBehaviour
{

    public List<GameObject> levelButtons = new List<GameObject>();

    private void Start()
    {
        Persistence.Load();
        for(int i = 0; i < gameObject.transform.childCount; i++ )
        {
            if (gameObject.transform.GetChild(i).tag.Contains("LevelButton"))
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

            levelButtons[i].transform.GetChild(0).GetComponent<Text>().text = Persistence.savedLevels[i].Number.ToString();
            levelButtons[i].transform.GetChild(1).GetComponent<Text>().text = Mathf.RoundToInt(Persistence.savedLevels[i].Time).ToString() + "s";

            int levelIndex = Persistence.savedLevels[i].Number;
            levelButtons[levelIndex - 1].GetComponent<Button>().interactable = true;

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