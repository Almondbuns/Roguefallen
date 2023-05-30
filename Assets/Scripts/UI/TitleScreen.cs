using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    
    public void StartNewGame()
    {
        GameObject.Find("GameEngine").GetComponent<GameEngine>().StartNewGame();
        Destroy(this.gameObject);
    }

    public void LoadGame()
    {
        GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadGame();
        Destroy(this.gameObject);
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }
}
