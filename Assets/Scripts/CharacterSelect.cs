using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] characters;

    public void activateCharacter(int index, bool IsHappy)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
        characters[index].SetActive(true);


        ObstacleAnimationBlender animationBlender = characters[index].GetComponentInChildren<ObstacleAnimationBlender>();
        if (animationBlender != null)
        {
            print("Found it!: " + animationBlender);
            animationBlender.setIsHappy(IsHappy);
        }
        else
        {
            print("No found");
        }

        //Also change whether character should appear happy or not.

    }

    /*  public void setCharactersIsHappy(bool value)
     {
         for (int i = 0; i < characters.Length; i++)
         {

         }
     } */
}
