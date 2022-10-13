using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class HoldData
{
    public static int selectedCharacter { get; set; }
}

public class CharacterSelecting : MonoBehaviour
{
    // public static int selecedCharacter = 0;

    public GameObject[] characters;

    // Start is called before the first frame update
    void Start()
    {
        hideAllPlayers();
        HoldData.selectedCharacter = 0;
        characters[HoldData.selectedCharacter].SetActive(true);

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
        characters[HoldData.selectedCharacter].SetActive(false);
        HoldData.selectedCharacter++;
        if (HoldData.selectedCharacter >= characters.Length)
        {
            HoldData.selectedCharacter = 0;
        }

        characters[HoldData.selectedCharacter].SetActive(true);
        print("Char is" + HoldData.selectedCharacter);

    }

    public void prevCharacter()
    {
        characters[HoldData.selectedCharacter].SetActive(false);
        HoldData.selectedCharacter--;
        if (HoldData.selectedCharacter < 0)
        {
            HoldData.selectedCharacter = characters.Length - 1;
        }

        characters[HoldData.selectedCharacter].SetActive(true);
        print("Char is" + HoldData.selectedCharacter);

    }





}
