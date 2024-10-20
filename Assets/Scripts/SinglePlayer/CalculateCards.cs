using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class CalculateCards : MonoBehaviour
{
    public CardUIManager cardUIManager;
    public List<string> deck = new List<string>();
    public GameObject calculateUI;
    public SoloTimer timer;
    private int score;
    public TMP_Text yourHand;
    public TMP_Text yourScore;
    private int extraScore;
    public TMP_Text extraScoreText;

    void Start()
    {
        // 可以在启动时或通过某个事件来计算牌型
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            deck = cardUIManager.OutputDeck();
            score = CalculateHandScore(deck);
            GlobalVariables.Score += score;
            calculateUI.SetActive(true);
            string handRankName = GetHandRankName(score);
            yourHand.text = "Your hand: " + handRankName;
            yourScore.text = "Your score: " + score;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            //Debug.Log(GlobalVariables.Score);
            //Debug.Log($"Hand score: {score}");
            switch(cardUIManager.GetMissionIndex())
            {
                case 0:
                    break;
                case 1:
                    if(cardUIManager.GetCardNumByColor("Heart") >= 2)
                    {
                        extraScore = 30;
                    }
                    break;
                case 2:
                    break;
            }
            extraScoreText.text = "Extra score: " + extraScore;
            //Debug.Log("Extra score:" + extraScore);
        }
    }

    public int CalculateHandScore(List<string> cards)
    {
        if (cards.Count != 5)
        {
            Debug.LogError("必须有五张牌来计算牌型！");
            return 0;
        }

        // 解析并排序
        var hand = cards
            .OrderBy(card => int.Parse(card.Substring(1)))
            .ToList();

        if (IsRoyalFlush(hand)) return (int)HandRank.RoyalFlush;
        if (IsStraightFlush(hand)) return (int)HandRank.StraightFlush;
        if (IsFourOfAKind(hand)) return (int)HandRank.FourOfAKind;
        if (IsFullHouse(hand)) return (int)HandRank.FullHouse;
        if (IsFlush(hand)) return (int)HandRank.Flush;
        if (IsStraight(hand)) return (int)HandRank.Straight;
        if (IsThreeOfAKind(hand)) return (int)HandRank.ThreeOfAKind;
        if (IsTwoPair(hand)) return (int)HandRank.TwoPair;
        if (IsOnePair(hand)) return (int)HandRank.OnePair;

        return (int)HandRank.HighCard;
    }

    private bool IsRoyalFlush(List<string> hand)
    {
        // 首先检查是否为同花顺
        if (!IsStraightFlush(hand))
            return false;

        // 提取牌的数值部分
        var values = hand.Select(card => int.Parse(card.Substring(1))).ToList();

        // 如果有 A（01），处理成两种情况：作为 1 或 14
        bool hasLowAce = values.Contains(1);
        bool hasHighCards = values.Contains(10) && values.Contains(11) && values.Contains(12) && values.Contains(13);

        // 检查皇家同花顺（10, J, Q, K, A）
        return hasLowAce && hasHighCards;
    }
    private bool IsStraightFlush(List<string> hand) => IsFlush(hand) && IsStraight(hand);
    private bool IsFourOfAKind(List<string> hand) => hand.GroupBy(card => card.Substring(1)).Any(group => group.Count() == 4);
    private bool IsFullHouse(List<string> hand) => hand.GroupBy(card => card.Substring(1)).Count() == 2 && hand.GroupBy(card => card.Substring(1)).Any(group => group.Count() == 3);
    private bool IsFlush(List<string> hand) => hand.All(card => card[0] == hand[0][0]);
    private bool IsStraight(List<string> hand)
    {
        var values = hand.Select(card => int.Parse(card.Substring(1))).ToList();

        // 如果有 A（01），处理成两种情况：作为 1 或 14
        bool isLowAceStraight = false;
        if (values.Contains(1))
        {
            // 将 A 视为 14
            var highAceValues = values.Select(v => v == 1 ? 14 : v).OrderBy(v => v).ToList();
            // 将 A 视为 1
            var lowAceValues = values.OrderBy(v => v).ToList();

            // 检查顺子：两种情况都需要检查
            bool isHighAceStraight = highAceValues.Distinct().Count() == 5 && highAceValues.Max() - highAceValues.Min() == 4;
            isLowAceStraight = lowAceValues.SequenceEqual(new List<int> { 1, 2, 3, 4, 5 });

            return isHighAceStraight || isLowAceStraight;
        }

        // 普通顺子情况
        values = values.OrderBy(v => v).ToList();
        bool isStandardStraight = values.Distinct().Count() == 5 && values.Max() - values.Min() == 4;

        return isStandardStraight;
    }
    private bool IsThreeOfAKind(List<string> hand) => hand.GroupBy(card => card.Substring(1)).Any(group => group.Count() == 3);
    private bool IsTwoPair(List<string> hand) => hand.GroupBy(card => card.Substring(1)).Count(group => group.Count() == 2) == 2;
    private bool IsOnePair(List<string> hand) => hand.GroupBy(card => card.Substring(1)).Any(group => group.Count() == 2);

    private enum HandRank
    {
        HighCard = 0,
        OnePair = 10,
        TwoPair = 25,
        ThreeOfAKind = 40,
        Straight = 80,
        Flush = 100,
        FullHouse = 200,
        FourOfAKind = 400,
        StraightFlush = 800,
        RoyalFlush = 2000
    }


    private string GetHandRankName(int score)
    {
        switch (score)
        {
            case (int)HandRank.RoyalFlush:
                return "Royal Flush";
            case (int)HandRank.StraightFlush:
                return "Straight Flush";
            case (int)HandRank.FourOfAKind:
                return "Four of a Kind";
            case (int)HandRank.FullHouse:
                return "Full House";
            case (int)HandRank.Flush:
                return "Flush";
            case (int)HandRank.Straight:
                return "Straight";
            case (int)HandRank.ThreeOfAKind:
                return "Three of a Kind";
            case (int)HandRank.TwoPair:
                return "Two Pair";
            case (int)HandRank.OnePair:
                return "One Pair";
            default:
                return "High Card";
        }
    }

    public void ReloadScene()
    {
        Debug.Log("BUtton pressed");
        Time.timeScale = 1f;
        GlobalVariables.Score = timer.remainingDuration + score + extraScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
