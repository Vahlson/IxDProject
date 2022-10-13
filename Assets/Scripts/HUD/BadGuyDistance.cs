using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BadGuyDistance : MonoBehaviour
{
    [SerializeField]
    private Image _badGuyImage;
    [SerializeField]
    private GameObject _timer;
    [SerializeField]
    private Image _playerImage;
    private Player _player;
    private Badguy _badguy;
    private float _lastDistance = .0f;
    private float _xDistance = 0.0f;
    private float _xStep = 0.0f;
    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _badguy = GameObject.FindObjectOfType<Badguy>();
        _lastDistance = Mathf.Abs(_player.totalDistanceTravelled - _badguy.totalDistanceTravelled);
        _xDistance = Mathf.Abs(_playerImage.transform.localPosition.x - _badGuyImage.transform.localPosition.x);
        _xStep = _xDistance / 100;
    }

    void Update()
    {
        UpdateDistance();
        // print("xdistance = " + _xDistance);
        // _badGuyImage.transform.localPosition = new Vector3(_xDistance, _badGuyImage.transform.localPosition.y, _badGuyImage.transform.localPosition.z);
        // print("Badguysimage pos " + _badGuyImage.transform.localPosition.x);

    }

    void UpdateDistance()
    {
        float currentDistance = Mathf.Abs(_player.totalDistanceTravelled - _badguy.totalDistanceTravelled);
        print("current distance = " + currentDistance);
        if (currentDistance > 100)
        {
            _badGuyImage.transform.localPosition = new Vector3(0, _badGuyImage.transform.localPosition.y, _badGuyImage.transform.localPosition.z);

        }
        else
        {

            _badGuyImage.transform.localPosition = new Vector3(_xDistance / currentDistance, _badGuyImage.transform.localPosition.y, _badGuyImage.transform.localPosition.z);
            print("current pos " + _badGuyImage.transform.localPosition);
            print("current div " + _xDistance / currentDistance);


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
