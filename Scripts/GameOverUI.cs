using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GameOverUI : MonoBehaviour
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
        _textUi.text = "Plants Killed: " + gameManager.Score;
    }
}