using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTimer : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TMP_Text uiText;

    public int Duration;

    private int remainingDuration;
    void Start()
    {
        Being(Duration);
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
            remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
    }

    public void AddTime(int seconds)
    {
        remainingDuration += seconds;
        uiText.text = $"{remainingDuration / 60:00} : {remainingDuration % 60:00}";
        uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
    }
}
