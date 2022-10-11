using Unity;
using UnityEngine;
using TMPro;
class NewHighScoreScreen : MonoBehaviour
{
    [SerializeField]
    private LetterSpinner[] spinners;
    public TMP_Text scoreText;
    public GameObject leaderboardScreen;
    private string _name = "AAA";
    void Start()
    {
        scoreText.text = "You placed " + getPositionText() + " with " + GameManager.Instance.latestScore + " points!";
        foreach (var item in spinners)
        {
            item.GetComponent<LetterSpinner>().OnTextChanged += UpdateText;
        }
        leaderboardScreen.GetComponent<LeaderboardScreen>().CreateLeaderboardEntries();
    }
    void Update()
    {
        //change to something else later, maybe a blinking border or something.
        leaderboardScreen.GetComponent<LeaderboardScreen>().rotateNewHighScore();
    }
    void OnDestroy()
    {
        foreach (var item in spinners)
        {
            item.GetComponent<LetterSpinner>().OnTextChanged -= UpdateText;
        }
    }

    private string getPositionText()
    {
        int position = GameManager.Instance.latestPlacement;
        switch (position)
        {
            case 1:
                return position + "st";
            case 2:
                return position + "nd";
            case 3:
                return position + "rd";
            default: return position + "th";
        }
    }

    private void UpdateText()
    {
        _name = "";
        foreach (var item in spinners)
        {
            _name += item.letter;
        }
        leaderboardScreen.GetComponent<LeaderboardScreen>().UpdateEntryName(_name);
        GameManager.Instance.UpdateName(_name);


    }
    public void SaveScore()
    {
        _name = "";
        foreach (var item in spinners)
        {
            _name += item.letter;
        }
        GameManager.Instance.UpdateNewLeaderboardScoreName(_name);
        GameManager.Instance.SaveHighScore();

    }
}