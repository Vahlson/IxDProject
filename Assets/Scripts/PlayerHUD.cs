using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public TMP_Text score;
    public TMP_Text highScore;
    private int _highScore = 0;
    private Player _player;
    [SerializeField]
    private GameObject _livesContainer;
    [SerializeField]
    private GameObject _healthModel;


    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _highScore = PlayerPrefs.GetInt("HighScore");
        highScore.text = "Highscore:" + _highScore.ToString();
        for (int i = 0; i < _player.currentHealth; i++)
        {
            GameObject g = Instantiate(_healthModel, _livesContainer.transform.position, Quaternion.identity);
            g.transform.SetParent(_livesContainer.transform);
            g.transform.localScale = new Vector3(100, 100, 100);
            g.transform.localPosition += new Vector3(i * 100, 0, 0);
            g.transform.Rotate(new Vector3(0, 90, 0));

        }

    }

    ///todo change so that highscore is set after game is over perhaps?
    void Update()
    {
        score.text = "Score: " + _player.score.ToString();
        if (_player.score >= _highScore)
        {
            _highScore = _player.score;
            highScore.text = "HighScore: " + _highScore;
            PlayerPrefs.SetInt("HighScore", _highScore);
        }
    }
}
