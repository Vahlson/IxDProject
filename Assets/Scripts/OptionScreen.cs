using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionScreen : MonoBehaviour
{
    public Toggle fullscreenlog, option2, option3;

    // Start is called before the first frame update
    void Start()
    {
        fullscreenlog.isOn = Screen.fullScreen;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
