using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    private GameObject _scoreTextContainer;
    [SerializeField]
    private GameObject _scoreTextAnimation;
    public TMP_Text score;
    public TMP_Text bpm;
    public TMP_Text highScore;
    [SerializeField]
    private GameObject _livesContainer;
    [SerializeField]
    private GameObject _healthModel;
    private Stack<GameObject> _health = new Stack<GameObject>();
    private int currentHealth = 0;
    private int _highScore = 0;
    private Player _player;
    private Badguy _badguy;
    [SerializeField]
    private StanceIndicator _stanceIndicator;


    void OnObstacleDodged(float score)
    {
        GameObject g = Instantiate(_scoreTextAnimation, Vector3.zero, Quaternion.identity);
        g.transform.parent = _scoreTextContainer.transform;
        g.transform.localPosition = Vector3.zero;
        g.transform.localScale = Vector3.one;
        g.transform.eulerAngles = Vector3.one;
        g.GetComponent<TextAnimation>().setRandomizedPosition();

    }
    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _badguy = GameObject.FindObjectOfType<Badguy>();
        _player.OnStanceChanged += OnStanceChanged;
        _player.OnObstacleDodged += OnObstacleDodged;
        _highScore = PlayerPrefs.GetInt("HighScore");
        highScore.text = "Highscore:" + _highScore.ToString();
        initHealthIndicators();
    }

    void Update()
    {
        score.text = "Score: " + ((int)_player.score).ToString();
        bpm.text = "BPM: " + _player.getBPM();
        if (_player.score >= _highScore)
        {
            _highScore = (int)_player.score;
            highScore.text = "NEW HIGHSCORE: " + _highScore;
            PlayerPrefs.SetInt("HighScore", _highScore);
        }
        while (_player.currentHealth < currentHealth)
        {
            currentHealth--;
            GameObject g = _health.Pop();
            Destroy(g);
        }
    }
    void OnDestroy()
    {
        _player.OnStanceChanged -= OnStanceChanged;
        _player.OnObstacleDodged -= OnObstacleDodged;


    }
    private void initHealthIndicators()
    {
        for (int i = 0; i < _player.currentHealth; i++)
        {
            GameObject g = Instantiate(_healthModel, _livesContainer.transform.position, Quaternion.identity);
            g.transform.SetParent(_livesContainer.transform);
            g.transform.localScale = new Vector3(50, 50, 50);
            g.transform.localPosition += new Vector3(i * 110, 0, 0);
            g.transform.eulerAngles = new Vector3(270, 0, 0);
            _health.Push(g);
        }
        currentHealth = _player.currentHealth;
    }
    void OnStanceChanged(PlayerStance playerStance)
    {
        switch (playerStance)
        {
            case PlayerStance.high:
                _stanceIndicator.setHigh(_player.stanceDuration);
                break;
            case PlayerStance.medium:

                _stanceIndicator.setMedium(_player.stanceDuration);
                break;
            case PlayerStance.low:
                _stanceIndicator.setLow(_player.stanceDuration);
                break;
            case PlayerStance.idle:

                _stanceIndicator.setIdle();
                break;
        }
    }

}
