using UnityEngine;

public class FlameCollider : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private float startVelocity = 5;
    [SerializeField] private float life = 2;
    [SerializeField] private AnimationCurve velocityOverLife;

    private float lifeTime;

    private void Start()
    {
        lifeTime = 0;
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        var velocityAdjustment = velocityOverLife.Evaluate(lifeTime / life);
        transform.position += transform.TransformDirection(new Vector3(0, 0, startVelocity * velocityAdjustment * Time.deltaTime));

        if (lifeTime >= life)
        {
            Destroy(gameObject);
        }
    }
}