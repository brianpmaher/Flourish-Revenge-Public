using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject mainCanvas;

    private void Awake()
    {
        gameManager.onGamePaused.AddListener(OpenMenu);
        gameManager.onGameResumed.AddListener(CloseMenu);
    }

    private void OpenMenu()
    {
        mainCanvas.SetActive(true);
    }

    private void CloseMenu()
    {
        mainCanvas.SetActive(false);
    }
}