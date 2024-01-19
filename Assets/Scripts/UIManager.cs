using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class UIManager : MonoBehaviour
{
    [SerializeField]
    RectTransform _endMenuUI;

    [SerializeField]
    TMP_Text _finalScore;

    [SerializeField]
    TMP_Text _totalScore;

    [SerializeField]
    TMP_Text _score;

    [SerializeField]
    TMP_Text _multiplier;

    [SerializeField]
    TMP_Text _multipliedScore;

    [SerializeField]
    TMP_Text _timeLeft;

    [SerializeField]
    Color _colorTimeRunningOut;

    public void UpdateTotalScore(int num)
    {
        _totalScore.text = num.ToString();
    }

    public void UpdateScore(int num)
    {
        _score.text = num.ToString();
    }

    public void UpdateMultiplier(float num)
    {
        _multiplier.text = "x"+ num.ToString();
    }

    public void UpdateMultipliedScore(int num)
    {
        _multipliedScore.text = num.ToString();
    }

    public void UpdateTimeLeft(float num)
    {
        _timeLeft.text = (num < 10 ? "0" : "") + num.ToString("n2");
    }

    public void ChangeTimeColor()
    {
        _timeLeft.color = _colorTimeRunningOut;
    }

       // High score functionality 
    public HighScoreManager highScoreManager;
    
    public TextMeshProUGUI highScoreText;

    public GameObject newHighScoreText; // Assuming this is a GameObject with Text component

    /* Max's addition, stars */

    
    public Image star1;
    public Image star2;
    public Image star3;
    
    public Sprite greyStarSprite;
    public Sprite coloredStarSprite;
    
    public int scoreThresholdForStar1 = 300;
    public int scoreThresholdForStar2 = 600;
    public int scoreThresholdForStar3 = 1000;
    public int temphighscore;

    public void UpdateStarDisplay(int currentScore)
    {
        star1.sprite = currentScore >= scoreThresholdForStar1 ? coloredStarSprite : greyStarSprite;
        star2.sprite = currentScore >= scoreThresholdForStar2 ? coloredStarSprite : greyStarSprite;
        star3.sprite = currentScore >= scoreThresholdForStar3 ? coloredStarSprite : greyStarSprite;
    }

    // Call this method when the score updates.
  
    /* end of Max's addition, stars */



    public void OnNewScore(int score)
        {
        if (highScoreManager.TrySetNewHighScore(score))
            {
            newHighScoreText.SetActive(true); // Show "NEW" when a new high score is set
            highScoreText.text = ManagerLocator.Instance.Stats.TotalPoints.ToString();
            }
            else
            {
                temphighscore = highScoreManager.GetHighScore();
                highScoreText.text = temphighscore.ToString();
            }
        }

    // End Menu UI
    public void StartEndMenu()
    {
        _endMenuUI.gameObject.SetActive(true);

        /*max added */
        UpdateStarDisplay(ManagerLocator.Instance.Stats.TotalPoints);
        OnNewScore(ManagerLocator.Instance.Stats.TotalPoints);

        _finalScore.text = ManagerLocator.Instance.Stats.TotalPoints.ToString();

        float _growingTime = 0.5f;
        Vector3 _minScale = new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 _originalScale = _endMenuUI.localScale;

        _endMenuUI.localScale = _minScale;

        _endMenuUI.DOScale(_originalScale, _growingTime)
                    .SetEase(Ease.InOutSine);
    }
}
