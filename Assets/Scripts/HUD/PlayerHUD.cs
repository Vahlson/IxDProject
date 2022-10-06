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

    void OnDestroy()
    {
        _player.OnStanceChanged -= OnStanceChanged;

    }
    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _player.OnStanceChanged += OnStanceChanged;
        _highScore = PlayerPrefs.GetInt("HighScore");
        highScore.text = "Highscore:" + _highScore.ToString();
        initHealthIndicators();
        initStance(currentStance, new Vector3(0, 90, 0));
    }
    private void initStance(GameObject model, Vector3 rotation)
    {
        currentStance = Instantiate(model, _stanceContainer.transform.position, Quaternion.identity);
        currentStance.transform.SetParent(_stanceContainer.transform);
        currentStance.transform.localScale = new Vector3(100, 100, 100);
        currentStance.transform.localPosition = Vector3.zero;
        currentStance.transform.Rotate(rotation);
        this._stanceIndicator = currentStance.GetComponent<StanceIndicator>();
    }
    private void initHealthIndicators()
    {
        for (int i = 0; i < _player.currentHealth; i++)
        {
            GameObject g = Instantiate(_healthModel, _livesContainer.transform.position, Quaternion.identity);
            g.transform.SetParent(_livesContainer.transform);
            g.transform.localScale = new Vector3(100, 100, 100);
            g.transform.localPosition += new Vector3(i * 100, 0, 0);
            g.transform.Rotate(new Vector3(0, 90, 0));
            _health.Push(g);
        }
        currentHealth = _player.currentHealth;
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
    void OnStanceChanged(PlayerStance playerStance)
    {
        switch (playerStance)
        {
            case PlayerStance.high:
                // initStance(_highStance, new Vector3(0, 130, 0));
                _stanceIndicator.setHigh();
                currentStance.transform.eulerAngles = new Vector3(0, 130, 0);
                // currentStance.GetComponent<StanceIndicator>().setHigh();
                break;
            case PlayerStance.medium:
                // initStance(_midStance, new Vector3(0, 130, 0));
                currentStance.transform.eulerAngles = new Vector3(0, 130, 0);

                _stanceIndicator.setMedium();
                break;
            case PlayerStance.low:
                // initStance(_lowStance, new Vector3(0, 90, 0));
                currentStance.transform.eulerAngles = new Vector3(0, 90, 0);

                _stanceIndicator.setLow();
                break;
            case PlayerStance.idle:
                // initStance(_idleStance, new Vector3(0, 90, 0));
                currentStance.transform.eulerAngles = new Vector3(0, 90, 0);

                _stanceIndicator.setIdle();
                break;
        }
    }
}
