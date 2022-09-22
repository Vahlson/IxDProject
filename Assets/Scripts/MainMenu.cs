using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string firstLevel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void openOptions()
    {

    }

    public void closeOptions()
    {

    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
