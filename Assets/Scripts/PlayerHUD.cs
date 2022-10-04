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

    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _highScore = PlayerPrefs.GetInt("HighScore");
        highScore.text = "Highscore:" + _highScore.ToString();
        initHealthIndicators();
        // setStanceIndicators(_highStance, _stanceContainer, new Vector3(0, 0, 0));
    }
    private void setStanceIndicators(GameObject model, GameObject parent, Vector3 position)
    {
        GameObject g = Instantiate(model, parent.transform.position, Quaternion.identity);
        g.transform.SetParent(parent.transform);
        g.transform.localScale = new Vector3(100, 100, 100);
        g.transform.localPosition += position;
        g.transform.Rotate(new Vector3(0, 90, 0));
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
        checkPlayerStance();
    }
    void checkPlayerStance()
    {
        //requires bool to check if there is a new playerstance.
        
        //make idle mode.
        switch (_player.stance)
        {
            case PlayerStance.high:
                //set player stance

                break;
            case PlayerStance.medium:

                break;
            case PlayerStance.low:

                break;
            case PlayerStance.idle:

                break;
        }
    }
}
