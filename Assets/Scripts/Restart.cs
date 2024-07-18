using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    public Button btnRestart;
    
    void Start()
    {
        btnRestart.onClick.AddListener(ReStart);
    }

    private void ReStart()
    {
        SceneManager.LoadScene(0);
    }
} // end class
