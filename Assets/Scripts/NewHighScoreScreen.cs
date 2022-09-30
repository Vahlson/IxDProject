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
    void Update()
    {
        if (GameManager.Instance._newLeaderboardEntry != null)
        {
            leaderboardScreen.GetComponent<LeaderboardScreen>().rotateNewHighScore();
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
    void Start()
    {

        print("latest score is:" + GameManager.Instance.latestScore);
        scoreText.text = "You placed " + getPositionText() + " with " + GameManager.Instance.latestScore + "!";
        foreach (var item in spinners)
        {
            item.GetComponent<LetterSpinner>().OnTextChanged += UpdateText;
        }
        leaderboardScreen.GetComponent<LeaderboardScreen>().CreateLeaderboardEntries();

    }
    void OnDestroy()
    {
        foreach (var item in spinners)
        {
            item.GetComponent<LetterSpinner>().OnTextChanged -= UpdateText;
        }
    }
    void UpdateText()
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
        foreach (var item in spinners)
        {
            _name += item.letter;
        }
        GameManager.Instance._newLeaderboardEntry.name = _name;
        GameManager.Instance.SaveHighScore();

    }
}