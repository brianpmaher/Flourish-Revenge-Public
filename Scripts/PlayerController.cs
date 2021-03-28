using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public UnityEvent onDie;
    
    [Header("Dependencies")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private Fireable weapon;
    [SerializeField] private Damageable damageable;
    [SerializeField] private GameManager gameManager;

    [Header("Config")] 
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float lookSpeed = 3;
    [SerializeField] private AudioClip walkingAudioClip;

    [Header("Input")] 
    [SerializeField] private InputAction onMove;
    [SerializeField] private InputAction onLook;
    [SerializeField] private InputAction onFire;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;
    private bool isDead;
    private AudioSource _audioSource;
    private static readonly int MovementForward = Animator.StringToHash("MovementForward");
    private static readonly int MovementRight = Animator.StringToHash("MovementRight");
    private static readonly int AnimateDie = Animator.StringToHash("Die");

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        
        // Handle inputs
        onMove.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        onMove.canceled += ctx => moveDirection = Vector2.zero;
        onLook.performed += ctx => lookDirection = ctx.ReadValue<Vector2>();
        onLook.canceled += ctx => lookDirection = Vector2.zero;
        onFire.performed += _ => weapon.StartFiring();
        onFire.canceled += _ => weapon.StopFiring();
    }

    private void OnEnable()
    {
        damageable.onDie.AddListener(Die);

        gameManager.onGameResumed.AddListener(() =>
        {
            onMove.Enable();
            onLook.Enable();
            onFire.Enable();
        });

        gameManager.onGamePaused.AddListener(() =>
        {
            onMove.Disable();
            onLook.Disable();
            onFire.Disable();
        });
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuel"))
        {
            var fuel = other.GetComponent<Fuel>().PickUp();
            weapon.IncreaseFuel(fuel);
        }
    }

    private void Update()
    {
        if (isDead) return;
        Move();
        Look();
    }

    private void OnDisable()
    {
        damageable.onDie.RemoveListener(Die);
        
        onMove.Disable();
        onLook.Disable();
        onFire.Disable();
    }

    private void Move()
    {
        var normalizedMovement = moveDirection * (moveSpeed * Time.deltaTime);
        var localMovement = new Vector3(normalizedMovement.x, 0, normalizedMovement.y);
        var globalMovement = transform.TransformDirection(localMovement);
        
        characterController.Move(globalMovement);
        animator.SetFloat(MovementForward,  localMovement.normalized.z);
        animator.SetFloat(MovementRight,  localMovement.normalized.x);

        if (localMovement.normalized.magnitude > 0)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = walkingAudioClip;
                _audioSource.Play();
            }
        }
        else if (_audioSource.clip == walkingAudioClip)
        {
            _audioSource.Stop();
        }
    }

    private void Look()
    {
        var normalizedLook = lookDirection * (lookSpeed * Time.deltaTime);
        transform.Rotate(0, normalizedLook.x, 0);
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool(AnimateDie, true);
        Time.timeScale = 0.1f;
        onDie.Invoke();
    }
}
