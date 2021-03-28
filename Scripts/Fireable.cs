using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Fireable : MonoBehaviour
{
    [HideInInspector] public UnityEvent<float> onFuelChanged;
    
    [Header("Dependencies")]
    [SerializeField] private VisualEffect flamethrowerEffect;
    [SerializeField] private AudioSource flamethrowerSoundEffect;
    [SerializeField] private GameObject flameColliderPrefab;
    [SerializeField] private Transform nozzleTransform;

    [Header("Config")] 
    [SerializeField] private float collidersPerSecond = 2;
    [SerializeField] private float fuelConsumedPerSecond = 5;
    [SerializeField] private float maxFuel = 100;

    private bool isFiring;
    private float fuelLevel = 100;

    public float MaxFuel => maxFuel;
    public float FuelLevel => fuelLevel;
    
    private void Awake()
    {
        flamethrowerEffect.Stop();
        flamethrowerSoundEffect.Stop();
    }

    public void Start()
    {
        FireFuelChangedEvent();
    }

    public void StartFiring()
    {
        if (fuelLevel > 0)
        {
            isFiring = true;
            flamethrowerEffect.Play();
            flamethrowerSoundEffect.Play();
            StartCoroutine(SpawnFlameColliders());
        }
    }

    public void StopFiring()
    {
        isFiring = false;
        flamethrowerEffect.Stop();
        flamethrowerSoundEffect.Stop();
        StopCoroutine(SpawnFlameColliders());
    }

    private IEnumerator SpawnFlameColliders()
    {
        var flameCollider = Instantiate(flameColliderPrefab);
        flameCollider.transform.position = nozzleTransform.position;
        flameCollider.transform.rotation = nozzleTransform.rotation;

        var seconds = 1 / collidersPerSecond; // .5
        fuelLevel -= seconds * fuelConsumedPerSecond;
        if (fuelLevel < 0)
        {
            fuelLevel = 0;
        }
        FireFuelChangedEvent();
        
        yield return new WaitForSeconds(seconds);
        
        if (isFiring && fuelLevel > 0)
        {
            yield return StartCoroutine(SpawnFlameColliders());
        }
        else
        {
            StopFiring();
        }
    }

    private void FireFuelChangedEvent()
    {
        onFuelChanged.Invoke(fuelLevel);
    }

    public void IncreaseFuel(float fuel)
    {
        fuelLevel += fuel;
        
        if (fuelLevel > maxFuel)
        {
            fuelLevel = maxFuel;
        }
        
        FireFuelChangedEvent();
    }
}