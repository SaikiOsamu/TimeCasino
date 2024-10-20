using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class Timer : NetworkBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TMP_Text uiText;

    public int Duration;

    private GameObject nwUI;
    private NetworkManagerUI nwUIScript;

    private int remainingDuration;
    void Start()
    {
        Being(Duration);
        nwUI = GameObject.Find("Canvas");
        nwUIScript = nwUI.GetComponent<NetworkManagerUI>();
    }

    private void Being(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(Updatetimer());
    }

    private IEnumerator Updatetimer()
    {
        while(remainingDuration >= 0)
        {
            uiText.text = $"{remainingDuration / 60:00} : {remainingDuration % 60:00}";
            uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
            //uiFill.fillAmount = remainingDuration / Duration;
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Minus Player");
        nwUIScript.MinusPlayer();
    }

    public void AddTime(int seconds)
    {
        if(remainingDuration + seconds > Duration)
        {
            remainingDuration = Duration;
        }
        else
        {
            remainingDuration += seconds;
        }
        uiText.text = $"{remainingDuration / 60:00} : {remainingDuration % 60:00}";
        uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
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
        uiText.text = $"{remainingDuration / 60:00} : {remainingDuration % 60:00}";
        uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
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
