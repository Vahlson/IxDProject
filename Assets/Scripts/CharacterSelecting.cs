using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelecting : MonoBehaviour
{
    int selecedCharacter = 0;

    public GameObject[] characters;
    // Start is called before the first frame update
    void Start()
    {
        hideAllPlayers();
        selecedCharacter = 0;
        characters[selecedCharacter].SetActive(true);

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
        characters[selecedCharacter].SetActive(false);
        selecedCharacter++;
        if (selecedCharacter >= characters.Length)
        {
            selecedCharacter = 0;
        }

        characters[selecedCharacter].SetActive(true);

    }

    public void prevCharacter()
    {
        characters[selecedCharacter].SetActive(false);
        selecedCharacter--;
        if (selecedCharacter < 0)
        {
            selecedCharacter = characters.Length - 1;
        }

        characters[selecedCharacter].SetActive(true);

    }

}
