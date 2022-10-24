using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public string firstLevel;

    private ArduinoInputController arduinoInputController;
    private int currentSteps = 0;



    [SerializeField] Transform progressBar;
    [SerializeField] RectTransform progressBarImage;
    [SerializeField] Transform runningIcon;

    [SerializeField] private CharacterSelecting characterSelecting;

    /* 
        [SerializeField] private float targetBPM = 100f; */

    void Start()
    {
        this.arduinoInputController = GetComponent<ArduinoInputController>();
    }

    // Update is called once per frame
    void Update()
    {




        if (arduinoInputController.getKeyDown(5) || arduinoInputController.getKeyDown(4))
        {
            step();

        }

        if (characterSelecting != null)
        {
            if (arduinoInputController.getKeyDown(6))
            {
                //right
                characterSelecting.nextCharacter();
            }
            if (arduinoInputController.getKeyDown(3))
            {
                //left
                characterSelecting.prevCharacter();
            }
        }
        // if (arduinoInputController.getKeyDown(6))
        // {
        // }
        // if (arduinoInputController.getKeyDown(5))
        // {
        // }
        // if (arduinoInputController.getKeyDown(4))
        // {
        // }
        if (characterSelecting != null)
        {
            if (Input.GetKeyUp("d"))
            {
                characterSelecting.nextCharacter();
            }
            if (Input.GetKeyUp("a"))
            {
                characterSelecting.prevCharacter();
            }
        }



        if (Input.GetKeyDown("h") || Input.GetKeyDown("j"))
        {

            step();
            //tramplingSpeed += GameManager.Instance.stepSpeedIncrease;
        }

        /* float bpmOfTarget = getBPM() / targetBPM;
        print(bpmOfTarget); */
        float tramplingFactor = (GameManager.Instance.tramplingSpeed / GameManager.Instance.maxTramplingSpeed) * 2;

        progressBar.localScale = new Vector3(tramplingFactor, progressBar.localScale.y, progressBar.localScale.z);
        runningIcon.localPosition = new Vector3(progressBar.localScale.x * progressBarImage.rect.width + 100, runningIcon.localPosition.y, runningIcon.localPosition.z);

        if (tramplingFactor >= 1)
        {
            startGame();
        }

        //smoothCurrentStep = Mathf.Clamp(smoothCurrentStep,0,GameManager.Instance.maxTramplingSpeed);

        /* while (steps.Count > 0 && Time.realtimeSinceStartup - steps.Peek() >= nSeconds)
        {
            steps.Dequeue();
            currentSteps--;

        }
        _progressIndicator.transform.localPosition = new Vector3(currentSteps * _stepSize, _progressIndicator.transform.localPosition.y, _progressIndicator.transform.localPosition.z);
        if (currentSteps >= 10)
        {
            startGame();
        } */

        //recountBPM();
    }



    public void startGame()
    {
        //GameManager.Instance.mainMenuAchievedSteps = GameManager.Instance.steps;
        /* GameManager.Instance.timeBeforeSceneSwitch = Time.realtimeSinceStartup;
        GameManager.Instance.shouldDequeSteps = Time.realtimeSinceStartup; */
        SceneManager.LoadScene(firstLevel);
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }


    void step()
    {
        GameManager.Instance.step();
    }
    /* void recountBPM()
    {
        //print(steps.Count);
        GameManager.Instance.recountBPM();

    }
    public int getBPM()
    {
        return GameManager.Instance.getBPM();
    } */
}
