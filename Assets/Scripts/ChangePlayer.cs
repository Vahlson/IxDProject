using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayer : MonoBehaviour
{


    public Image startLeft;
    public Image startSelected;

    public Image startRight;

    int order;
    int fitList;
    int restartList;

    public Sprite[] nextImg;
    // Start is called before the first frame update
    void Start()
    {
        order = 0;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeSpriteLeft()
    {
        order++;


        startLeft.sprite = nextImg[order];

        if (order > nextImg.Length - 2)
        {
            print("2");
            startSelected.sprite = nextImg[0];
            startRight.sprite = nextImg[1];
        }


        else if (order > nextImg.Length - 3)
        {
            print("first");
            startSelected.sprite = nextImg[order + 1];
            startRight.sprite = nextImg[0];
        }
        else
        {
            print("3");

            startSelected.sprite = nextImg[order + 1];
            startRight.sprite = nextImg[order + 2];

        }

        if (order == (nextImg.Length - 1))
        {
            order = -1;
        }


    }

    public void changeSpriteRight()
    {
        order++;


        if (order == 0)
        {
            startLeft.sprite = nextImg[order];
            startSelected.sprite = nextImg[order + 1];
            startRight.sprite = nextImg[order + 2];
        }

        else if (order == (nextImg.Length - 1))
        {
            startLeft.sprite = nextImg[order - 2];
            startSelected.sprite = nextImg[order - 1];
            startRight.sprite = nextImg[order];
            order = -1;

        }

        else if (order == (nextImg.Length - 2))
        {
            startLeft.sprite = nextImg[order];
            startSelected.sprite = nextImg[order + 1];
            startRight.sprite = nextImg[0];

        }

        else
        {
            startLeft.sprite = nextImg[3];
            startSelected.sprite = nextImg[0];
            startRight.sprite = nextImg[1];
        }


    }


}
