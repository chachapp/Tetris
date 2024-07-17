using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score;
    public Spawner spawner;
    public TextMeshProUGUI text;
    
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject container = new GameObject("GameManager");
                    instance = container.AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }
    
    
    // 한 줄 삭제되면 게임매니저에 테트리스 점수
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void CreateNewTetromino()
    {
        Debug.Log("CreateNewTetromino");
        spawner?.CreateTetris();
    }

    public void AddScore()
    {
        score += 100;
        text.text = score.ToString();
    }
} // end class
