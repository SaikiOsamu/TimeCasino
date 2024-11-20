using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUIManager : MonoBehaviour
{
    public Image[] TopLeft;
    public Image[] BotRight;
    public Sprite[] Hearts;
    public Sprite[] Diamonds;
    public Sprite[] Clubs;
    public Sprite[] Spades;
    public Sprite emptyCard;
    public PickUpCard pickUpCard;
    public Sprite[] utilLogos;
    public Image utilImage;
    private int missionIndex;
    public TMP_Text missionText;

    private void Start()
    {
        for (int i = 0; i < TopLeft.Length; i++)
        {
            TopLeft[i].sprite = emptyCard;
        }

        for (int i = 0; i < BotRight.Length; i++)
        {
            BotRight[i].sprite = emptyCard;
        }
        GenerateDeck();
        UpdateUtilLogo(0);
    }

    public void ReplaceCard(string cardColor, int num)
    {
        switch (cardColor)
        {
            case "Heart":
                //Debug.Log(cardColor + num.ToString());
                //Debug.Log(CheckBotEmpty());
                if (CheckBotEmpty() != -1)
                {
                    //Debug.Log(cardColor + num.ToString());
                    BotRight[CheckBotEmpty()].sprite = Hearts[num - 1];
                }
                break;
            case "Spade":
                //Debug.Log(cardColor + num.ToString());
                //Debug.Log(CheckBotEmpty());
                if (CheckBotEmpty() != -1)
                {
                    //Debug.Log(cardColor + num.ToString());
                    BotRight[CheckBotEmpty()].sprite = Spades[num - 1];
                }
                break;
            case "Clubs":
                //Debug.Log(cardColor + num.ToString());
                //Debug.Log(CheckBotEmpty());
                if (CheckBotEmpty() != -1)
                {
                    //Debug.Log(cardColor + num.ToString());
                    BotRight[CheckBotEmpty()].sprite = Clubs[num - 1];
                }
                break;
            case "Diamond":
                //Debug.Log(cardColor + num.ToString());
                //Debug.Log(CheckBotEmpty());
                if (CheckBotEmpty() != -1)
                {
                    //Debug.Log(cardColor + num.ToString());
                    BotRight[CheckBotEmpty()].sprite = Diamonds[num - 1];
                }
                break;

        }

    }

    public int CheckBotEmpty()
    {
        if (BotRight[0].sprite == emptyCard)
        {
            return 0;
        }
        else if (BotRight[1].sprite == emptyCard)
        {
            return 1;
        }
        return -1;
    }

    public void DropLeft()
    {
        BotRight[0].sprite = emptyCard;
    }

    public void DropRight()
    {
        BotRight[1].sprite = emptyCard;
    }

    public int[,] RandomDeck()
    {
        /*int[,] num = new int[3, 2];
        List<string> usedCombinations = new List<string>(); // ���ڴ洢�Ѿ����ɵ����

        for (int i = 0; i < 3; i++)
        {
            int firstRandom, secondRandom;
            string combination;

            do
            {
                firstRandom = Random.Range(0, 4); // ��0��3�����������
                secondRandom = Random.Range(0, 13); // ��0��12�����������
                combination = firstRandom + "-" + secondRandom; // ���Ϊһ���ַ��������ڼ���ظ�
                num[i, 0] = firstRandom;
                num[i, 1] = secondRandom;
            }
            while (usedCombinations.Contains(combination)); // �������Ѵ��ڣ�����������

            // ���������ӵ������б���
            usedCombinations.Add(combination);

        }
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(num[i, 0].ToString() + " " + num[i, 1].ToString());
        }
        return num;*/
        //Cheat
        int[,] num = new int[3, 2];

        // ���ף�ֱ�����ɺ���A������Q������10
        num[0, 0] = 0; // ����
        num[0, 1] = 0; // A (������0��ʼ)
        num[1, 0] = 0; // ����
        num[1, 1] = 11; // Q (������0��ʼ)
        num[2, 0] = 0; // ����
        num[2, 1] = 9; // 10 (������0��ʼ)

        for (int i = 0; i < 3; i++)
        {
            Debug.Log(num[i, 0].ToString() + " " + num[i, 1].ToString());
        }
        return num;
    }

    public void GenerateDeck()
    {
        int[,] num = RandomDeck();
        for (int i = 0; i < 3; i++)
        {
            if (num[i, 0] == 0)
            {
                TopLeft[i].sprite = Hearts[num[i, 1]];
            }
            else if (num[i, 0] == 1)
            {
                TopLeft[i].sprite = Spades[num[i, 1]];
            }
            else if (num[i, 0] == 2)
            {
                TopLeft[i].sprite = Clubs[num[i, 1]];
            }
            else if (num[i, 0] == 3)
            {
                TopLeft[i].sprite = Diamonds[num[i, 1]];
            }
        }
    }

    public void UpdateUtilLogo(int index)
    {
        utilImage.sprite = utilLogos[index];
    }

    public List<string> OutputDeck()
    {
        List<string> deck = new List<string>();
        foreach (Image i in TopLeft)
        {
            string[] temp = i.sprite.name.Split('_');
            deck.Add((temp[0][0]).ToString() + (temp[1]));
        }
        foreach (Image i in BotRight)
        {
            string[] temp = i.sprite.name.Split('_');
            deck.Add((temp[0][0]).ToString() + (temp[1]));
        }
        //Debug.Log(deck);
        return deck;
    }

    public string GetCard(int index)
    {
        Debug.Log(OutputDeck()[index]);
        return OutputDeck()[index];

    }

    public int GetMissionIndex()
    {
        return missionIndex;
    }


    public int GetCardNumByColor(string color)
    {
        int num = 0;
        foreach (Image i in TopLeft)
        {
            if (i.sprite.name.Contains(color))
            {
                num++;
            }
        }
        foreach (Image i in BotRight)
        {
            if (i.sprite.name.Contains(color))
            {
                num++;
            }
        }
        return num;
    }

    public void GetRandomMission()
    {
        missionIndex = Random.Range(0, 3);
        switch (missionIndex)
        {
            case 0:
                missionText.text = "Mission: Pick up 2 Spades(Bonus: 30s)";
                break;
            case 1:
                missionText.text = "Mission: Pick up 2 Hearts(Bonus: 30s)";
                break;
            case 2:
                missionText.text = "Mission: Pick up 2 Clubs(Bonus: 30s)";
                break;
        }
    }

}