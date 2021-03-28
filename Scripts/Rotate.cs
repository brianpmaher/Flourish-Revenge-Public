using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private float speed = 180;

    private void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}