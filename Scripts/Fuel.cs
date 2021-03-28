using System.Collections;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private GameObject canisterPrefab;
    
    [Header("Config")] 
    [SerializeField] private float fuelAmount = 100;
    [SerializeField] private float respawnTime = 30;
    
    private GameObject canister;
    
    private void Start()
    {
        canister = transform.GetChild(0).gameObject;
    }

    public float PickUp()
    {
        if (canister != null)
        {
            Destroy(canister);
            StartCoroutine(RespawnCanister());
            return fuelAmount;
        }

        return 0;
    }

    private IEnumerator RespawnCanister()
    {
        yield return new WaitForSeconds(respawnTime);
        var newCanister = Instantiate(canisterPrefab, transform, true);
        newCanister.transform.localPosition = new Vector3(0, 0.398f, 0);
        newCanister.transform.localRotation = Quaternion.Euler(-45, 0, 0);
        newCanister.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        canister = newCanister;
    }
}