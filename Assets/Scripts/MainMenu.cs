using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public string firstLevel;

    private ArduinoInputController arduinoInputController;
    //[SerializeField]
    //private Image _progressBar;
    /* [SerializeField]
    private Image _progressIndicator; */
    private float _stepSize;
    public int nSteps = 10;
    public int nSeconds = 10;
    private Queue<float> steps = new Queue<float>();
    private int currentSteps = 0;
    private float _endpos;


    private float smoothCurrentStep;
    [SerializeField] private float currentStepDecreaseSpeed = 0.1f;

    [SerializeField] Transform progressBar;
    [SerializeField] RectTransform progressBarImage;
    [SerializeField] Transform runningIcon;

    [SerializeField] private CharacterSelecting characterSelecting;

    void Start()
    {
        this.arduinoInputController = GetComponent<ArduinoInputController>();
        //_endpos = _progressBar.rectTransform.rect.width - _progressIndicator.rectTransform.rect.width;
        _stepSize = _endpos / nSteps;
    }

    // Update is called once per frame
    void Update()
    {
        smoothCurrentStep -= Time.deltaTime * currentStepDecreaseSpeed;
        smoothCurrentStep = Mathf.Clamp(smoothCurrentStep, 0f, 1f);

        if (arduinoInputController.getKeyDown(1) || arduinoInputController.getKeyDown(2))
        {
            steps.Enqueue(Time.realtimeSinceStartup);
            currentSteps++;

            smoothCurrentStep += 0.1f;
        }

        if (characterSelecting != null)
        {
            if (arduinoInputController.getKeyDown(3))
            {
                //right
                characterSelecting.nextCharacter();
            }
            if (arduinoInputController.getKeyDown(0))
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
            steps.Enqueue(Time.realtimeSinceStartup);
            currentSteps++;

            smoothCurrentStep += 0.1f;
        }

        progressBar.localScale = new Vector3(smoothCurrentStep, progressBar.localScale.y, progressBar.localScale.z);
        runningIcon.localPosition = new Vector3(progressBar.localScale.x * progressBarImage.rect.width + 100, runningIcon.localPosition.y, runningIcon.localPosition.z);
        if (smoothCurrentStep >= 1)
        {
            startGame();
        }

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
    }

    public void startGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
