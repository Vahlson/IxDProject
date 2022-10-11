using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    [SerializeField]
    Player _player;
    public Vector3 offset = Vector3.zero;
    void Start()
    {
    }

    void Update()
    {
        this.transform.rotation = _player.transform.rotation;
        this.transform.position = _player.transform.position + offset;
    }
}
