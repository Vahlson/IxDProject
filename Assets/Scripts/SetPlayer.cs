using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayer : MonoBehaviour
{

    public GameObject[] playerCharacters;
    private CharacterSelecting characterSelecting;
    private int chosenPlayer;
    // Start is called before the first frame update
    void Start()
    {
        hideCharactersNotUSed();
        playerCharacters[GameManager.Instance.selectedCharacter].SetActive(true);


    }

    // Update is called once per frame
    void Update()
    {

    }


    void hideCharactersNotUSed()
    {

        print(playerCharacters);
        foreach (GameObject child in playerCharacters)
        {
            child.SetActive(false);

        }

    }
}
