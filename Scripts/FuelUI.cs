using TMPro;
using UnityEngine;

public class FuelUI : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Fireable flamethrower;
    [SerializeField] private TextMeshProUGUI testUi;

    private void OnEnable()
    {
        flamethrower.onFuelChanged.AddListener(HandleFuelChanged);
    }

    private void OnDisable()
    {
        flamethrower.onFuelChanged.RemoveListener(HandleFuelChanged);
    }

    private void HandleFuelChanged(float fuelLevel)
    {
        testUi.text = "Fuel level: " + fuelLevel;
    }
}