using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIHealthBar : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Damageable playerHealth;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private Sprite healthFullSprite;
    [SerializeField] private Sprite healthEmptySprite;

    private List<GameObject> _healthIndicators = new List<GameObject>();

    private void Start()
    {
        var maxHealth = playerHealth.MaxHealth;

        for (var i = 0; i < maxHealth; i++)
        {
            var health = Instantiate(healthPrefab, transform);
            health.transform.localPosition = new Vector3(80 * i, 0);
            health.GetComponent<Image>().sprite = healthFullSprite;
            _healthIndicators.Add(health);
        }
    }

    private void OnEnable()
    {
        playerHealth.onHealthChanged.AddListener(UpdateHealthBar);
    }

    private void OnDisable()
    {
        playerHealth.onHealthChanged.RemoveListener(UpdateHealthBar);
    }

    private void UpdateHealthBar(int health)
    {
        var currentHealth = playerHealth.CurrentHealth;
        var maxHealth = playerHealth.MaxHealth;

        for (var i = 0; i < maxHealth; i++)
        {
            var healthImage = _healthIndicators[i].GetComponent<Image>();
            healthImage.sprite = i < currentHealth ? healthFullSprite : healthEmptySprite;
        }
    }
}