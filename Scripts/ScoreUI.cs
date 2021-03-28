using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreUI : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private GameManager gameManager;

    private TextMeshProUGUI _textUi;

    private void Awake()
    {
        _textUi = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        gameManager.onScoreChanged.AddListener(SetScoreText);
    }

    private void OnDisable()
    {
        gameManager.onScoreChanged.RemoveListener(SetScoreText);
    }

    private void SetScoreText(int score)
    {
        _textUi.text = "Plants Killed: " + score;
    }
}