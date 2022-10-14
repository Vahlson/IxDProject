using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterSelecting : MonoBehaviour
{

    public GameObject[] characters;

    void Start()
    {
        hideAllPlayers();
        GameManager.Instance.selectedCharacter= 0;
        characters[GameManager.Instance.selectedCharacter].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void hideAllPlayers()
    {
        foreach (GameObject g in characters)
        {
            g.SetActive(false);
        }
    }

    public void nextCharacter()
    {
        characters[GameManager.Instance.selectedCharacter].SetActive(false);
        GameManager.Instance.selectedCharacter++;
        if (GameManager.Instance.selectedCharacter >= characters.Length)
        {
            GameManager.Instance.selectedCharacter = 0;
        }

        characters[GameManager.Instance.selectedCharacter].SetActive(true);
        print("Char is" + GameManager.Instance.selectedCharacter);

    }

    public void prevCharacter()
    {
        characters[GameManager.Instance.selectedCharacter].SetActive(false);
        GameManager.Instance.selectedCharacter--;
        if (GameManager.Instance.selectedCharacter < 0)
        {
            GameManager.Instance.selectedCharacter = characters.Length - 1;
        }

        characters[GameManager.Instance.selectedCharacter].SetActive(true);
        print("Char is" + GameManager.Instance.selectedCharacter);

    }





}
