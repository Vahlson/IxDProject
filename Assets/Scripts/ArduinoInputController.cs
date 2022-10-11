using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        print("damessage: " + message);
        //Parse the message.
        bool[] input = parseArduinoMessage(message);


        if (input[0] == true)
        {
            moveCube(-1);
        }
        else if (input[1] == true)
        {
            moveCube(1);
        }

        string temp = "";
        foreach (bool b in input)
        {
            temp += b + ", "; //maybe also + '\n' to put them on their own line.
        }


        /* if (input[1] == trueBit)
        {
            moveCube();
        } */

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

    bool[] parseArduinoMessage(string message)
    {
        //TODO might want to remove first bit, the one at position 0, because it is likely a stop bit.
        List<int> charList = new List<int>();
        foreach (char c in message)
        {
            int number = int.TryParse(c.ToString(), out int i) ? i : -1;
            charList.Add(number);
            print("char: " + c);
        }

        // bit 0 is a stopbit. REMOVE IT
        charList.RemoveAt(0);

        string word = charList.Select(i => i.ToString()).Aggregate((i, j) => i + j);
        print("The numberrrrrr: " + word);
        int messageBit = int.Parse(word);
        //int messageBit = charList[1];

        //The least significant will be stored first.
        BitArray bitArray = new BitArray(new int[] { messageBit });
        bool[] bits = new bool[bitArray.Count];
        bitArray.CopyTo(bits, 0);
        //The arduino sends 0 to represent button being pushed, we want the opposite
        bool[] invertedBits = (bool[])bits.Clone();



        return bits;
    }

    void moveCube(int direction)
    {
        print("MOVE");
        transform.Translate(Vector3.right * 0.1f * direction);

    }


}
