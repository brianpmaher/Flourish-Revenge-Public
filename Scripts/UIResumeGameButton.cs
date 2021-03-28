using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIResumeGameButton : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private GameManager gameManager;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ResumeGame);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ResumeGame);
    }

    private void ResumeGame()
    {
        gameManager.ResumeGame();
    }
}