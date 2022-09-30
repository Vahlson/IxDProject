using Unity;
using UnityEngine;
using TMPro;
class ScoreScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private GameObject _leaderboardScreen;
    void Start()
    {
        _scoreText.text = "Your score is: " + GameManager.Instance.latestScore;
    }

}