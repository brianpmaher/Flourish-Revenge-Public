using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent<int> onScoreChanged;
    
    [Header("Dependencies")] 
    [SerializeField] private GameObject uiHud;
    [SerializeField] private GameObject uiGameOver;
    [SerializeField] private Button uiRetryButton;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject uiStartScreen;
    [SerializeField] private GameObject uiCreditScreen;
    
    [Header("Inputs")] 
    [SerializeField] private InputAction onCancel;

    public UnityEvent onGamePaused;
    public UnityEvent onGameResumed;

    private bool GamePaused => Time.timeScale == 0;
    private int _score;
    private bool _gameOver;
    
    public int Score => _score;

    private void Awake()
    {
        onCancel.performed += _ => HandleCancel();
    }

    private void OnEnable()
    {
        onCancel.Enable();
        
        uiRetryButton.onClick.AddListener(RestartGame);
        playerController.onDie.AddListener(HandleGameOver);
    }

    private void Start()
    {
        _score = 0;
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        onCancel.Disable();
        
        uiRetryButton.onClick.RemoveListener(RestartGame);
        playerController.onDie.RemoveListener(HandleGameOver);
    }

    public void IncreaseScore(int score)
    {
        _score += score;
        onScoreChanged.Invoke(_score);
    }
    
    private void HandleCancel()
    {
        if (!GamePaused && !_gameOver)
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        uiStartScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        onGameResumed.Invoke();
        uiHud.SetActive(true);
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        onGamePaused.Invoke();
    }

    private void HandleGameOver()
    {
        _gameOver = true;
        uiHud.SetActive(false);
        uiGameOver.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowCredits()
    {
        uiStartScreen.SetActive(false);
        uiCreditScreen.SetActive(true);
    }

    public void ReturnToStart()
    {
        uiCreditScreen.SetActive(false);
        uiStartScreen.SetActive(true);
    }
}