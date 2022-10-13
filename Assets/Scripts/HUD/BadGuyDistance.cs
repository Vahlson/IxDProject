using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BadGuyDistance : MonoBehaviour
{
    [SerializeField]
    private Image _badGuyImage;
    [SerializeField]
    private GameObject _timer;
    [SerializeField]
    private Image _playerImage;
    [SerializeField]
    private TMP_Text _badGuyDistanceText;
    private Player _player;
    private Badguy _badguy;
    private float _lastDistance = .0f;
    private float _xDistance = 0.0f;
    private float _xStep = 0.0f;
    private float _end = 0.0f;

    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _badguy = GameObject.FindObjectOfType<Badguy>();
        _end = _playerImage.transform.localPosition.x - _playerImage.rectTransform.rect.width;
        _lastDistance = Mathf.Abs(_player.totalDistanceTravelled - _badguy.totalDistanceTravelled);
        _xDistance = Mathf.Abs(_badguy.transform.localPosition.x - _end);
        _xStep = _xDistance / 100;
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.ongoing)
        {

            UpdateDistance();
        }
        // print("xdistance = " + _xDistance);
        // _badGuyImage.transform.localPosition = new Vector3(_xDistance, _badGuyImage.transform.localPosition.y, _badGuyImage.transform.localPosition.z);
        // print("Badguysimage pos " + _badGuyImage.transform.localPosition.x);

    }

    void UpdateDistance()
    {
        float currentDistance = Mathf.Abs(_player.totalDistanceTravelled - _badguy.totalDistanceTravelled);
        _badGuyDistanceText.text = ((int)currentDistance).ToString();

        if (currentDistance > 100)
        {
            _badGuyImage.transform.localPosition = new Vector3(0, _badGuyImage.transform.localPosition.y, _badGuyImage.transform.localPosition.z);

        }
        else
        {

            _badGuyImage.transform.localPosition = new Vector3(_xStep * (100 - currentDistance), _badGuyImage.transform.localPosition.y, _badGuyImage.transform.localPosition.z);
            if (_badGuyImage.transform.localPosition.x >= _end)
            {
                _badGuyImage.transform.localPosition = new Vector3(_end, _badGuyImage.transform.localPosition.y, _badGuyImage.transform.localPosition.z);
            }
        }
    }

    // void updateDistance()
    // {
    //     if (Mathf.Abs(_player.totalDistanceTravelled - _badguy.totalDistanceTravelled) < _lastDistance)
    //     {
    //         _badGuyImage.transform.localPosition += new Vector3(1, 0, 0) * (Mathf.Abs(_player.totalDistanceTravelled - _badguy.totalDistanceTravelled) / Mathf.Abs(_player.velocity - _badguy.velocity)) * Time.deltaTime;
    //         print("Moving badguy closer");
    //         if (_badGuyImage.transform.localPosition.x > _player.transform.localPosition.x)
    //         {
    //             _badGuyImage.transform.localPosition = new Vector3(_player.transform.localPosition.x, _badGuyImage.transform.localPosition.y, _badGuyImage.transform.localPosition.z);
    //         }
    //     }
    //     else if (_badGuyImage.transform.localPosition.x > 0)
    //     {
    //         _badGuyImage.transform.localPosition -= new Vector3(1, 0, 0) * (Mathf.Abs(_player.totalDistanceTravelled - _badguy.totalDistanceTravelled) / Mathf.Abs(_player.velocity - _badguy.velocity)) * Time.deltaTime;

    //         // _badGuyImage.transform.localPosition -= new Vector3(1, 0, 0) * Mathf.Abs(_player.velocity - _badguy.velocity) * Time.deltaTime;
    //         if (_badGuyImage.transform.localPosition.x <= 0)
    //         {
    //             _badGuyImage.transform.localPosition = new Vector3(0, _badGuyImage.transform.localPosition.y, _badGuyImage.transform.localPosition.z);
    //         }
    //         print("Moving badguy further");


    //     }
    //     _lastDistance = Mathf.Abs(_player.totalDistanceTravelled - _badguy.totalDistanceTravelled);
    // }
}
