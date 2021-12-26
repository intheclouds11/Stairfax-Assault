using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreUI;

    private void Start()
    {
        scoreUI.text = $"Score: {score}";
    }

    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;
        scoreUI.text = $"Score: {score}";
    }
}
