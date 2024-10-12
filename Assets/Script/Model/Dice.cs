using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public int MainRoll(List<int> wentRoom)
    {
        List<int> possibleRolls = new List<int>();

        for (int i = 0; i < 20; i++)
        {
            if (!wentRoom.Contains(i))
            {
                possibleRolls.Add(i);
            }
        }

        if (possibleRolls.Count == 0)
        {
            Debug.Log("All rooms visited, no more possible rolls.");
            return -1;
        }

        int randomIndex = Random.Range(0, possibleRolls.Count);
        int rolledNumber = possibleRolls[randomIndex];

        Debug.Log("Rolled: " + rolledNumber);

        return rolledNumber;
    }
}
