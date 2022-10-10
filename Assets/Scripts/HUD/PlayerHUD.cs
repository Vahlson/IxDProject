using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public TMP_Text score;
    public TMP_Text highScore;
    [SerializeField]
    private GameObject _livesContainer;
    [SerializeField]
    private GameObject _healthModel;
    private Stack<GameObject> _health = new Stack<GameObject>();
    private int currentHealth = 0;
    private int _highScore = 0;
    private Player _player;
    [SerializeField]
    private GameObject _stanceContainer;
    [SerializeField]
    private GameObject currentStance;
    private StanceIndicator _stanceIndicator;


    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _player.OnStanceChanged += OnStanceChanged;
        _highScore = PlayerPrefs.GetInt("HighScore");
        highScore.text = "Highscore:" + _highScore.ToString();
        initHealthIndicators();
        initStance(currentStance);
    }

    void Update()
    {
        score.text = "Score: " + ((int)_player.score).ToString();

        if (_player.score >= _highScore)
        {
            _highScore = (int)_player.score;
            highScore.text = "NEW HIGHSCORE: " + _highScore;
            PlayerPrefs.SetInt("HighScore", _highScore);
        }
        if (_player.currentHealth < currentHealth)
        {
            currentHealth--;
            GameObject g = _health.Pop();
            Destroy(g);
        }
    }
    void OnDestroy()
    {
        _player.OnStanceChanged -= OnStanceChanged;

    }
    private void initStance(GameObject model)
    {
        currentStance = Instantiate(model, _stanceContainer.transform.position, Quaternion.identity);
        currentStance.transform.SetParent(_stanceContainer.transform);
        currentStance.transform.localScale = new Vector3(100, 100, 100);
        currentStance.transform.localPosition = Vector3.zero;
        currentStance.transform.Rotate(Vector3.zero);
        this._stanceIndicator = currentStance.GetComponent<StanceIndicator>();
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
