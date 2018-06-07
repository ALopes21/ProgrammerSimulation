using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    public int SceneNumber;
    int StarNumber;
    float myTime;
    public bool Unclocked;
    public int lives;

    public GameObject[] Stars;
    public GameObject[] Lives;
    public Sprite Star, NoStar, EmptyLife, FullLife;
    public GameObject ErrorPanel;
    public bool gameOver;

    public GameObject[] InvSlots;
    public GameObject[] Items;

    void Start()
    {
        ErrorPanel = GameObject.Find("ErrorPanel");
        ErrorPanel.SetActive(false);

        Star = (Sprite)Resources.Load("UISprites/Star", typeof(Sprite));
        NoStar = (Sprite)Resources.Load("UISprites/NoStar", typeof(Sprite));

        EmptyLife = (Sprite)Resources.Load("UISprites/EmptyLife", typeof(Sprite));
        FullLife = (Sprite)Resources.Load("UISprites/FullLife", typeof(Sprite));

        Stars = GameObject.FindGameObjectsWithTag("InGameStars");
        Lives = GameObject.FindGameObjectsWithTag("InGameLives");

        InvSlots = GameObject.FindGameObjectsWithTag("Slot");
        Items = GameObject.FindGameObjectsWithTag("Item");

        StarNumber = 3;
        lives = 3;
        gameOver = false;
        for (int i= 0; i < Stars.Length; i++)
        {
            Stars[i].GetComponent<Image>().sprite = Star;
        }
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
        if (myTime < 5f)
        {
            StarNumber = 3;
        }
        else if (myTime > 5f && myTime < 10f)
        {
            StarNumber = 2;
            Stars[0].GetComponent<Image>().sprite = NoStar;
        }
        else if (myTime > 10f)
        {
            StarNumber = 1;
            Stars[1].GetComponent<Image>().sprite = NoStar;
        }

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
                Debug.Log("OldStars: " + Persistence.savedLevels[i].Stars);
                if(Persistence.savedLevels[i].Stars < StarNumber)
                {
                    newLevel.Stars = StarNumber;
                    Debug.Log("NewStars: " + newLevel.Stars);
                }
                else
                {
                    newLevel.Stars = Persistence.savedLevels[i].Stars;
                    Debug.Log("SameAs: " + newLevel.Stars);
                }

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
                newLevel.Stars = StarNumber;
                newLevel.Time = myTime;
                newLevel.currentLevel = SceneNumber++;
                Persistence.UpdateLevelList(LevelData.current);
                return;
            }
        }
        Debug.Log("Creating FIRST save...");
        newLevel.Stars = StarNumber;
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
}

