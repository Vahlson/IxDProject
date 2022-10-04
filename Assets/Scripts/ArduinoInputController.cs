using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArduinoInputController : MonoBehaviour
{

    public int trueBit = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnConnectionEvent(bool connected)
    {
        print("Connected: " + connected);
    }

    void OnMessageArrived(string message)
    {
        //print(message);
        //Parse the message.
        List<int> input = parseArduinoMessage(message);

        string temp = "";
        foreach (int c in input)
        {
            temp += c + ", "; //maybe also + '\n' to put them on their own line.
        }

        print(input[1]);
        if (input[1] == trueBit)
        {
            moveCube();
        }

        print("Lista: " + temp);
        //print("Message: "+ message +" LOL:"+ float.Parse(message));
        // if (System.Int32.Parse(message) == 0)
        // {
        //print("j"+j);
        /* if(j == 1){
            moveCube();
        } */

        // }


    }

    List<int> parseArduinoMessage(string message)
    {
        //TODO might want to remove first bit, the one at position 0, because it is likely a stop bit.
        List<int> charList = new List<int>();
        foreach (char c in message)
        {
            int number = int.TryParse(c.ToString(), out int i) ? i : -1;
            charList.Add(number);
            //print("char: "+ c);
        }

        return charList;
    }

    void moveCube()
    {
        print("MOVE");
        transform.Translate(Vector3.right * 0.1f);

    }


}
