using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UIFuelLevelBar : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Fireable flamethrower;
    
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        flamethrower.onFuelChanged.AddListener(HandleFuelChanged);
    }

    private void Start()
    {
        _slider.value = flamethrower.FuelLevel / flamethrower.MaxFuel;
    }

    private void HandleFuelChanged(float fuelLevel)
    {
        _slider.value = fuelLevel / flamethrower.MaxFuel;
    }
}