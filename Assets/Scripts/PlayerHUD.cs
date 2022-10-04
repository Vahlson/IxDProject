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
    private GameObject _highStance;
    [SerializeField]
    private GameObject _midStance;
    [SerializeField]
    private GameObject _lowStance;
    [SerializeField]
    private GameObject _idleStance;
    private GameObject currentStance;
    void OnStanceChanged(PlayerStance playerStance)
    {
        switch (playerStance)
        {
            case PlayerStance.high:
                setCurrentStance(_highStance);
                break;
            case PlayerStance.medium:
                setCurrentStance(_midStance);
                break;
            case PlayerStance.low:
                setCurrentStance(_lowStance);
                break;
            case PlayerStance.idle:
                setCurrentStance(_idleStance);
                break;
        }
    }
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
        setCurrentStance(_idleStance);
    }
    private void setCurrentStance(GameObject model)
    {
        if (currentStance != null)
        {
            Destroy(currentStance);
        }
        currentStance = Instantiate(model, _stanceContainer.transform.position, Quaternion.identity);
        currentStance.transform.SetParent(_stanceContainer.transform);
        currentStance.transform.localScale = new Vector3(100, 100, 100);
        currentStance.transform.localPosition = Vector3.zero;
        currentStance.transform.Rotate(new Vector3(0, 90, 0));
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

}
