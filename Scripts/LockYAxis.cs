using UnityEngine;

public class LockYAxis : MonoBehaviour
{
    private void LateUpdate()
    {
        var position = transform.position;
        position.y = 0;
        transform.position = position;
    }
}