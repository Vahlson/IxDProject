using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BadGuyDistance : MonoBehaviour
{
    [SerializeField]
    private GameObject _badGuyIndicator;
    [SerializeField]
    private GameObject _playerIndicator;
    [SerializeField]
    private RectTransform _playerIndicatorRectTransform;
    [SerializeField]
    private TMP_Text _badGuyVelocityText;
    [SerializeField]
    private TMP_Text _playerVelocityText;
    [SerializeField]
    private Image _progressBar;
    private Player _player;
    private Badguy _badguy;
    private float _lastDistance = .0f;
    private float _xDistance = 0.0f;
    private float _xStep = 0.0f;
    private float _end = 0.0f;

    private float maxDistanceShown = 200;


    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _badguy = GameObject.FindObjectOfType<Badguy>();
        _end = _playerIndicator.transform.localPosition.x - _playerIndicatorRectTransform.rect.width;
        _lastDistance = Mathf.Abs(_player.totalDistanceTravelled - _badguy.totalDistanceTravelled);
        _xDistance = Mathf.Abs(_badguy.transform.localPosition.x - _end);
        _xStep = _xDistance / maxDistanceShown;
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.ongoing)
        {

            UpdateDistance();
            _playerVelocityText.text = (int)_player.velocity + "km/h";
            _badGuyVelocityText.text = (int)_badguy.velocity + "km/h";

        }

    }

    void UpdateDistance()
    {
        float currentDistance = Mathf.Abs(_player.totalDistanceTravelled - _badguy.totalDistanceTravelled);
        _playerVelocityText.text = ((int)currentDistance).ToString();

        if (currentDistance > maxDistanceShown)
        {
            _badGuyIndicator.transform.localPosition = new Vector3(0, _badGuyIndicator.transform.localPosition.y, _badGuyIndicator.transform.localPosition.z);
            _progressBar.transform.localScale = new Vector3(0, _progressBar.transform.localScale.y, _progressBar.transform.localScale.z);
        }
        else
        {

            _badGuyIndicator.transform.localPosition = new Vector3(_xStep * (maxDistanceShown - currentDistance), _badGuyIndicator.transform.localPosition.y, _badGuyIndicator.transform.localPosition.z);

            if (_badGuyIndicator.transform.localPosition.x >= _end)
            {
                _badGuyIndicator.transform.localPosition = new Vector3(_end, _badGuyIndicator.transform.localPosition.y, _badGuyIndicator.transform.localPosition.z);

            }
            _progressBar.transform.localScale = new Vector3((_badGuyIndicator.transform.localPosition.x + _playerIndicatorRectTransform.rect.width / 2) / _progressBar.rectTransform.rect.width, _progressBar.transform.localScale.y, _progressBar.transform.localScale.z);
            // _progressBar.GetComponent<Image>().pixelsPerUnitMultiplier =5;
        }
    }
}
