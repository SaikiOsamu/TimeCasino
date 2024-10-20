using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class SoloTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text uiText;
    public int entryFee;

    public int Duration;


    public int remainingDuration;
    void Start()
    {
        GlobalVariables.Score -= entryFee;
        Duration = GlobalVariables.Score;
        Being(Duration);
    }

    private void Being(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(Updatetimer());
    }

    private IEnumerator Updatetimer()
    {
        while (remainingDuration >= 0)
        {
            uiText.text = $"{remainingDuration}";
            //uiFill.fillAmount = remainingDuration / Duration;
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Minus Player");
    }

    public void AddTime(int seconds)
    {
        if (remainingDuration + seconds > Duration)
        {
            remainingDuration = Duration;
        }
        else
        {
            remainingDuration += seconds;
        }
        uiText.text = $"{remainingDuration}";
        //uiFill.fillAmount = remainingDuration / Duration;
    }

    public void RemoveTime(int seconds)
    {
        if (remainingDuration - seconds > 0)
        {
            remainingDuration -= seconds;
        }
        else
        {
            remainingDuration = 0;
        }
        uiText.text = $"{remainingDuration}";
    }


    public void RestoreTime()
    {
        remainingDuration = Duration;
    }

    public int GetRemainingDuration()
    {
        return remainingDuration;
    }

    public bool CheckIfDead()
    {
        if (remainingDuration <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
