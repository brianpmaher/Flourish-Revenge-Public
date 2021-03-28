using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIExitGameButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}