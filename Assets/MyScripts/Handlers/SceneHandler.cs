using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    public int SceneNumber;
    float myTime;
    public bool Unclocked;
    public int lives;

    public GameObject[] Lives;
    public Sprite EmptyLife, FullLife, InfoIcon;
    public GameObject ErrorPanel;
    public bool gameOver;
    public GameObject InfoPanel;
    public bool InfoModeActivated;

    public GameObject[] InvSlots;
    public GameObject[] Items;

    void Start()
    {
        ErrorPanel = GameObject.Find("ErrorPanel");
        ErrorPanel.SetActive(false);

        InfoPanel = GameObject.Find("InfoButton");
        InfoPanel.transform.GetChild(0).gameObject.SetActive(false);
        InfoIcon = (Sprite)Resources.Load("UISprites/InfoIcon", typeof(Sprite));
        InfoModeActivated = false;

        EmptyLife = (Sprite)Resources.Load("UISprites/EmptyLife", typeof(Sprite));
        FullLife = (Sprite)Resources.Load("UISprites/FullLife", typeof(Sprite));

        Lives = GameObject.FindGameObjectsWithTag("InGameLives");

        InvSlots = GameObject.FindGameObjectsWithTag("Slot");
        Items = GameObject.FindGameObjectsWithTag("Item");

        lives = 3;
        gameOver = false;

        for(int i = 0; i < Lives.Length; i++)
        {
            Lives[i].GetComponent<Image>().sprite = FullLife;
        }
    }

    void Update()
    {
        if(!gameOver)
        {
            IconHandler();
        }
    }

    public void IconHandler()
    {
        myTime += Time.deltaTime;
        GameObject.Find("ScorePanel").GetComponentInChildren<Text>().text = Mathf.RoundToInt(myTime).ToString();

        switch (lives)
        {
            case 0:
                foreach(GameObject icon in Lives)
                {
                    icon.GetComponent<Image>().sprite = EmptyLife;
                }
                ErrorPanel.SetActive(true);
                gameOver = true;
                break;
            case 1:
                Lives[0].GetComponent<Image>().sprite = FullLife;
                Lives[1].GetComponent<Image>().sprite = EmptyLife;
                Lives[2].GetComponent<Image>().sprite = EmptyLife;
                break;
            case 2:
                Lives[0].GetComponent<Image>().sprite = FullLife;
                Lives[1].GetComponent<Image>().sprite = FullLife;
                Lives[2].GetComponent<Image>().sprite = EmptyLife;
                break;
            case 3:
                foreach (GameObject icon in Lives)
                {
                    icon.GetComponent<Image>().sprite = FullLife;
                }
                break;
            default:
                break;
        }
    }

    public void UpdateLevelInfo()
    {
        LevelData newLevel = new LevelData();
        LevelData.current = newLevel;
        newLevel.Number = SceneNumber;

        for (int i = 0; i < Persistence.savedLevels.Count; i++)
        {
            if (Persistence.savedLevels[i].Number == SceneNumber)
            {              
                Debug.Log("OldTime: " + Persistence.savedLevels[i].Time);
                if (Persistence.savedLevels[i].Time > myTime)
                {
                    newLevel.Time = myTime;
                    Debug.Log("NewTime: " + newLevel.Time);
                }
                else
                {
                    newLevel.Time = Persistence.savedLevels[i].Time;
                    Debug.Log("SameAs: " + newLevel.Time);
                }
                newLevel.currentLevel = SceneNumber++;
                Persistence.UpdateLevelList(LevelData.current);
                return;
            }
            else
            {
                Debug.Log("Creating New Save...");
                newLevel.Time = myTime;
                newLevel.currentLevel = SceneNumber++;
                Persistence.UpdateLevelList(LevelData.current);
                return;
            }
        }
        Debug.Log("Creating FIRST save...");
        newLevel.Time = myTime;
        newLevel.currentLevel = SceneNumber++;
        Persistence.UpdateLevelList(LevelData.current);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ActivateInfoPanel()
    {
        if(InfoModeActivated == true)
        {
            InfoModeActivated = false;
            InfoPanel.GetComponentInChildren<Text>().text = "this is the info panel...";
            InfoPanel.transform.GetChild(0).gameObject.SetActive(false);
            InfoPanel.GetComponent<Image>().sprite = InfoIcon;
        }
        else if(InfoModeActivated == false)
        {
            InfoModeActivated = true;
            InfoPanel.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}

