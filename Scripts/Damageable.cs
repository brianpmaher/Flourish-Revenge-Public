using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [HideInInspector] public UnityEvent onDie;
    [HideInInspector] public UnityEvent<int> onHealthChanged;
    
    [Header("Config")] 
    [SerializeField] private int health = 1;

    private int _startHealth;

    public int MaxHealth => _startHealth;
    public int CurrentHealth => health;

    private void Awake()
    {
        _startHealth = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        onHealthChanged.Invoke(health);

        if (health <= 0)
        {
            onDie.Invoke();
        }
    }
}