using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Damageable damageable;
    [SerializeField] private Animator animator;
    [SerializeField] public GameObject player;
    [SerializeField] private Damager damager;
    [SerializeField] public GameManager gameManager;
    [SerializeField] public AudioSource hoppingAudioSource;
    [SerializeField] public AudioSource bitingAudioSource;

    [Header("Config")] 
    [SerializeField] private float attackingDistance = 1;
    [SerializeField] [Range(0, 1)] private float giveDamageOffset = .7f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private AnimationCurve hopAnimation = new AnimationCurve();

    private Damageable _playerDamageable;
    private bool _isAttacking;
    private bool _isMoving;
    private bool _isDead;
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    private static readonly int DieAnimation = Animator.StringToHash("Die");

    private void OnEnable()
    {
        damageable.onDie.AddListener(HandleDeath);
    }

    private void OnDisable()
    {
        damageable.onDie.RemoveListener(HandleDeath);
    }

    private void Start()
    {
        _playerDamageable = player.GetComponent<Damageable>();
    }

    private void Update()
    {
        if (_isDead)
        {
            if (_isMoving)
            {
                StopMoving();
            }

            if (_isAttacking)
            {
                StopAttacking();
            }
            
            return;
        }
        
        var distanceToPlayer = Vector3.Magnitude(transform.position - player.transform.position);

        if (distanceToPlayer <= attackingDistance)
        {
            if (!_isAttacking && !_isMoving)
            {
                StartAttacking();
            }

            if (_isMoving)
            {
                StopMoving();
            }
        }
        else
        {
            if (_isAttacking)
            {
                StopAttacking();
            }

            if (!_isMoving && !_isAttacking)
            {
                StartMoving();
            }
        }
    }

    private void StartMoving()
    {
        _isMoving = true;
        StartCoroutine(Move());
    }

    private void StopMoving()
    {
        _isMoving = false;
        StopCoroutine(Move());
        hoppingAudioSource.Stop();
    }
    
    private IEnumerator Move()
    {
        FacePlayer();
        hoppingAudioSource.Play();
        
        var ellapsedTime = 0f;
        while (ellapsedTime < 1f /* animation time */)
        {
            ellapsedTime += Time.deltaTime;
            var adjustment = hopAnimation.Evaluate(ellapsedTime) * Time.deltaTime;
            transform.position += moveSpeed * adjustment * transform.forward;
            yield return null;
            if (_isDead)
            {
                yield break;
            }
        }
        
        var distanceToPlayer = Vector3.Magnitude(transform.position - player.transform.position);
        if (distanceToPlayer > attackingDistance && !_isAttacking)
        {
            StartCoroutine(Move());
        }
    }
    
    private void StartAttacking()
    {
        _isAttacking = true;
        animator.SetBool(Attacking, true);
        StartCoroutine(Attack());
    }
    
    private void StopAttacking()
    {
        _isAttacking = false;
        animator.SetBool(Attacking, false);
    }
    
    private IEnumerator Attack()
    {
        FacePlayer();
        yield return new WaitForSeconds(giveDamageOffset);
        
        if (_isDead)
        {
            yield break;
        }
        
        bitingAudioSource.Play();
        
        var distanceToPlayer = Vector3.Magnitude(transform.position - player.transform.position);
        if (distanceToPlayer <= attackingDistance && !_isMoving)
        {
            _playerDamageable.TakeDamage(damager.damage);
            // Wait remainder of animation.
            yield return new WaitForSeconds(1 - giveDamageOffset);

            if (_isDead)
            {
                yield break;
            }
            
            StartCoroutine(Attack());
        }
    }

    private void FacePlayer()
    {
        transform.LookAt(player.transform.position);
    }

    private void HandleDeath()
    {
        if (_isDead)
        {
            return;
        }
        
        _isDead = true;
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        animator.SetBool(DieAnimation, true);
        gameManager.IncreaseScore(1);
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}