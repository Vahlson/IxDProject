using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
public class LetterSpinner : Selectable
{
    public TMP_Text tmp_letter;
    public string letter;
    private bool selected = false;
    [SerializeField]
    private GameObject topSpinner;
    [SerializeField]
    private GameObject bottomSpinner;
    private char[] _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private int _index = 0;

    public event Action OnTextChanged;

    override protected void Start()
    {
        base.Start();
        letter = "A";
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        selected = true;
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        selected = false;
    }
    void Update()
    {

        if (selected)
        {
            topSpinner.transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.Self);
            bottomSpinner.transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.Self);

            if (Input.GetKeyUp(KeyCode.F))
            {
                _index = (_index + 1) % _letters.Length;
                tmp_letter.text = _letters[_index % _letters.Length].ToString();
                letter = tmp_letter.text;
                OnTextChanged?.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                _index = (_index - 1) % _letters.Length;
                if (_index < 0)
                {
                    _index += _letters.Length;
                }
                tmp_letter.text = _letters[_index].ToString();
                letter = tmp_letter.text;
                OnTextChanged?.Invoke();
            }
        }

    }
}
