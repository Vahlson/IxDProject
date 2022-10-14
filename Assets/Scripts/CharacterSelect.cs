using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] characters;
    public void activateCharacter(int index)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
        characters[index].SetActive(true);
    }
}
