using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private bool inRound = true;

    private void Awake()
    {
        
    }

    public IEnumerator OutOfMissiles()
    {
        while (FindObjectsOfType<EvilMissile>().Length > 0)
        {
            yield return null;
        }

        inRound = false;
        yield return new WaitForSeconds(2f);
        LoadNextRound();
    }

    private void LoadNextRound()
    {
        inRound = true;
    }
}
