using System.Collections;
using UnityEngine;

public class Round
{
    int roundNumber = 1;

    public void StartRound()
    {

        Debug.Log("ROUND " + roundNumber);

        Debug.Log("Choose an action");
    }
    


    //public void TurnAction()
    //{
    //    Debug.Log("Choose an action");
    //}

    private void EndRound()
    {
        roundNumber++;
    }
}
