using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject gameManager;
    GameManager gameManagerScript;

    [HideInInspector] protected Rigidbody2D _rb;
    [SerializeField] protected CapsuleCollider2D _coll;
    [SerializeField] protected SteamManager _sm;
    [SerializeField] AudioClip _bulletClip;

    protected PlayerInput _input;
    protected InputStatus _inputStatus;

    protected Vector2 _internalSpeed;
    protected Vector2 _externalSpeed;
    protected int _currentFrame;
    public bool _hasControl = true;
    public float _pushBackTime; // Discuss these values with chris and turn them in constants.
    public float _pushBackForce;
    // external
    public event Action<bool, float> OnGroundedChange;
    public event Action<bool, Vector2> OnDashChange;
    public event Action OnJump; //boolean to differentiate wall jump
    public event Action OnAirJump;
    public event Action OnMelee;
    public event Action OnShoot;

    public Vector2 movement => _inputStatus.Move;
    public Vector2 speed => _internalSpeed;
    public Vector2 velocity => _rb.velocity;

    // Collisions
    protected bool _applyFriction;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] float _groundDistance;
    // [SerializeField] Vector2 _wallCheckSize;
    protected readonly RaycastHit2D[] _groundChecks = new RaycastHit2D[2];
    protected readonly RaycastHit2D[] _ceilChecks = new RaycastHit2D[2];
    protected int _frameLeftGround = int.MinValue;
    protected bool _grounded;
    protected Vector2 _skinWidth = new(0.02f, 0.02f);

    // Jumps

    protected bool _hasJump;
    protected bool _canJump;
    protected bool _jumpInterrupted;
    protected bool _canCoyote;
    protected int _frameJump;
    protected int _airJumpsLeft;

    // Dash 

    protected bool _hasDash;
    protected bool _canDash;
    protected bool _isDashing;
    protected int _dashFrame;
    protected float _dashTimer;
    protected Vector2 _dashVelocity;

    // Melee
    protected bool _hasMelee;
    protected int _lastAttackFrame = int.MinValue;

    // Shoot
    protected bool _hasShoot;
    protected int _lastShootFrame = int.MinValue;
    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected Transform _shootPoint;

    //constants
    protected const int JUMP_POWER = 28;

    protected const int JUMP_BUFFER = 5;
    protected const int COYOTE_BUFFER = 5;
    protected const int MAX_AIR_JUMPS = 0;
    protected const float DASH_COOLDOWN = 2f;
    protected const int DASH_VELOCITY = 40;
    protected const int DASH_DURATION = 6;
    protected const float DASH_HORIZONTAL_DECAY = .5f;
    protected const float X_AXIS_DEADZONE = .1f;
    protected const float Y_AXIS_DEADZONE = .1f;
    protected const int GROUND_SPEED_DECAY = 60;
    protected const int AIR_SPEED_DECAY = 30;
    protected const float FRICTION_MULTIPLIER = 1.5f;
    protected const int MAX_HORIZONTAL_SPEED = 8;
    protected const int HORIZONTAL_ACCELERATION = 100;
    protected const int MELEE_COOLDOWN_FRAMES = 15;
    protected const int SHOOT_COOLDOWN_FRAMES = 5;
    protected const float SHOOT_STEAM_PRICE = 0.3f;

    protected float _fallSpeedYDampingChangeThreshold;
    protected bool canAirJump => !_grounded && _airJumpsLeft > 0;

    #region debug checkpoints
    [SerializeField] protected Transform checkpoint1Transform;
    [SerializeField] protected Transform checkpoint2Transform;
    [SerializeField] protected Transform checkpoint3Transform;
    [SerializeField] protected Transform checkpoint4Transform;
    [SerializeField] protected Transform checkpoint5Transform;
    #endregion

    #region dialogue interactions
    public GameObject anchoredDialogue;
    protected bool _hasInteracted;
    #endregion

    public void ApplyVelocity(Vector2 vel, bool isDecay)
    {
        if (!isDecay) _internalSpeed += vel;
        else _externalSpeed += vel;
    }

    public void SetVelocity(Vector2 vel, bool isDecay)
    {
        if (!isDecay) _internalSpeed = vel;
        else _externalSpeed = vel;
    }

    public void TakeAwayControl(bool resetVelocity = true)
    {
        if (resetVelocity && _rb) _rb.velocity = Vector2.zero;
        _hasControl = false;
    }

    public void ReturnControl()
    {
        _internalSpeed = Vector2.zero;
        _hasControl = true;
    }

    protected void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        Physics2D.queriesStartInColliders = false;
    }

    protected void Start()
    {
        _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
    }

    protected void Update()
    {
        HandleDebugCheckpointInput();
        HandleInteraction();

        _inputStatus = _input.inputStatus;

        if (_inputStatus.JumpPushed)
        {
            _hasJump = true;
            _frameJump = _currentFrame;
            if (GameManager.Instance.gameState == "gameplay")
            {
                AudioManager.Instance.PlayEffect(AudioManager.Instance.jumpAudioClip);
            }
        }

        if (_inputStatus.Move.x != 0)
        {
            _applyFriction = false;
        }

        if (_inputStatus.DashPushed)
        {
            _hasDash = true;
        }

        if (_inputStatus.MeleePushed)
        {
            _hasMelee = true;
        }


        if (_inputStatus.ShootPushed)
        {
            _hasShoot = true;
        }
        if (_inputStatus.InteractPushed)
        {
            _hasInteracted = true;
        }
    }

    protected void HandleDebugCheckpointInput()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            gameObject.transform.position = checkpoint1Transform.position;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            gameObject.transform.position = checkpoint2Transform.position;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            gameObject.transform.position = checkpoint3Transform.position;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            gameObject.transform.position = checkpoint4Transform.position;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            gameObject.transform.position = checkpoint5Transform.position;
        }
    }

    protected void FixedUpdate()
    {
        _currentFrame++;
        HandleCameraLerp();
        HandleCollisions();
        HandleJump();
        HandleDash();
        HandleShoot();
        HandleMelee();

        HandleMovement();
        ApplyMovement();
    }

    protected void HandleCollisions()
    {
        Physics2D.queriesHitTriggers = false;

        int groundHits = Physics2D.CapsuleCastNonAlloc(_coll.bounds.center, _coll.size, _coll.direction, 0, Vector2.down, _groundChecks, _groundDistance, ~_playerLayer);
        int ceilHits = Physics2D.CapsuleCastNonAlloc(_coll.bounds.center, _coll.size, _coll.direction, 0, Vector2.up, _ceilChecks, _groundDistance, ~_playerLayer);

        Physics2D.queriesHitTriggers = true;

        if (ceilHits > 0)
        {
            _externalSpeed.y = Mathf.Min(0f, _externalSpeed.y);
            _internalSpeed.y = Mathf.Min(0, _internalSpeed.y);
        }

        if (!_grounded && groundHits > 0)
        {

            if (transform.parent == null)
            {
                foreach (RaycastHit2D item in _groundChecks)
                {
                    if (item && item.transform.CompareTag("MovingPlatform"))
                    {
                        transform.SetParent(item.transform);
                        break;
                    }
                }
            }

            _grounded = true;
            ResetDash();
            ResetJump();
            OnGroundedChange?.Invoke(true, Mathf.Abs(_internalSpeed.y));
            if (_inputStatus.Move.x == 0)
            {
                _applyFriction = true;
            }
        }
        else if (_grounded && groundHits == 0)
        {
            _grounded = false;
            _frameLeftGround = _currentFrame;
            OnGroundedChange?.Invoke(false, 0);
        }
    }

    protected void HandleJump()
    {
        if (!_jumpInterrupted && !_grounded && !_inputStatus.JumpHeld && _rb.velocity.y > 0) _jumpInterrupted = true;

        bool hasStoredJump = _canJump && _currentFrame < _frameJump + JUMP_BUFFER;
        if (!_hasJump && !hasStoredJump) return;

        bool hasCoyote = _canCoyote && !_grounded && _currentFrame < _frameLeftGround + COYOTE_BUFFER;

        if (_grounded || hasCoyote)
        {
            Jump();
        }

        else if (_hasJump && !_grounded && _airJumpsLeft > 0)
        {
            AirJump();
        }
        _hasJump = false;
    }

    protected void Jump()
    {
        _jumpInterrupted = false;
        _frameJump = 0;
        _canJump = false;
        _canCoyote = false;
        _internalSpeed.y = JUMP_POWER;
        OnJump?.Invoke();
    }

    protected void AirJump()
    {
        _jumpInterrupted = false;
        _airJumpsLeft--;
        _internalSpeed.y = JUMP_POWER;
        _externalSpeed.y = 0;
        // OnAirJump.Invoke(); Enable When needed
    }

    protected void ResetJump()
    {
        _canCoyote = true;
        _canJump = true;
        _jumpInterrupted = false;
        _airJumpsLeft = MAX_AIR_JUMPS;
    }

    protected void HandleDash()
    {
        if (_hasDash && _canDash && Time.time > _dashTimer)
        {
            Vector2 direction = new Vector2(_inputStatus.Move.x, Mathf.Max(_inputStatus.Move.y, 0f)).normalized;
            if (direction == Vector2.zero)
            {
                _hasDash = false;
                return;
            }

            _isDashing = true;
            _canDash = false;
            _dashFrame = _currentFrame;
            _dashTimer = Time.time + DASH_COOLDOWN;
            OnDashChange?.Invoke(true, direction);

            _dashVelocity = direction * DASH_VELOCITY;

            _externalSpeed = Vector2.zero;
        }
        if (_isDashing)
        {
            _internalSpeed = _dashVelocity;
            if (_currentFrame > _dashFrame + DASH_DURATION)
            {
                _isDashing = false;
                OnDashChange?.Invoke(false, Vector2.zero);
                _internalSpeed = new Vector2(_internalSpeed.x * DASH_HORIZONTAL_DECAY, Mathf.Min(0, _internalSpeed.y));
                if (_grounded) ResetDash();
            }
        }
        _hasDash = false;
    }

    protected void ResetDash()
    {
        _canDash = true;
    }

    protected virtual bool TryGetGroundNormal(out Vector2 groundNormal)
    {
        Physics2D.queriesHitTriggers = false;
        var hit = Physics2D.Raycast(_rb.position, Vector2.down, _groundDistance * 2, ~_playerLayer);
        Physics2D.queriesHitTriggers = true;
        groundNormal = hit.normal;
        return hit.collider;
    }

    protected void HandleMovement()
    {
        if (_isDashing) return;
        if (!(Mathf.Abs(_inputStatus.Move.x) > X_AXIS_DEADZONE))
        {
            float deceleration = _grounded ? GROUND_SPEED_DECAY * (_applyFriction ? FRICTION_MULTIPLIER : 1) : AIR_SPEED_DECAY;
            _internalSpeed.x = Mathf.MoveTowards(_internalSpeed.x, 0, deceleration * Time.fixedDeltaTime);

        }
        else
        {
            //TODO: prevent speed built up when colliding with walls
            _internalSpeed.x = Mathf.MoveTowards(_internalSpeed.x, _inputStatus.Move.x * MAX_HORIZONTAL_SPEED, HORIZONTAL_ACCELERATION * Time.fixedDeltaTime);

        }

        if (_grounded && _internalSpeed.y <= 0f)
        {
            _internalSpeed.y = -1.5f; // TODO: establish grounding force
        }
        else
        {
            float airGravity = 110.0f; // TODO: establish air gravity
            if (_jumpInterrupted && _internalSpeed.y > 0) airGravity *= 2f; //TODO determine jump interrupted gravity modifier
            _internalSpeed.y = Mathf.MoveTowards(_internalSpeed.y, -40f, airGravity * Time.fixedDeltaTime); //TODO: determine maxFallSpeed
        }
    }

    protected void ApplyMovement()
    {
        if (!_hasControl) return;

        _rb.velocity = _internalSpeed + _externalSpeed;
        _externalSpeed = Vector2.MoveTowards(_externalSpeed, Vector2.zero, .2f * Time.fixedDeltaTime); //TODO determine external speed decay    
    }

    protected void HandleCameraLerp()
    {
        if (_rb.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }
        if (_rb.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }

    protected void HandleMelee()
    {
        if (!_hasMelee) return;
        if (_currentFrame > _lastAttackFrame + MELEE_COOLDOWN_FRAMES)
        {
            _lastAttackFrame = _currentFrame;
            OnMelee?.Invoke();
        }
        _hasMelee = false;
    }

    protected void HandleShoot()
    {
        if (!_hasShoot) return;
        if (_currentFrame > _lastShootFrame + SHOOT_COOLDOWN_FRAMES)
        {
            if (_sm.UseSteam(SHOOT_STEAM_PRICE))
            {
                _lastShootFrame = _currentFrame;
                GameObject temp = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
                temp.GetComponent<SteamBullet>().SetDirection(transform.rotation.y == 0);
                AudioManager.Instance.PlayEffect(_bulletClip);
                OnShoot?.Invoke();
            }
        }
        _hasShoot = false;
    }

    public void ApplyPushBack(Vector2 direction, Boolean isDamage = false)
    {
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        _hasControl = false;
        _rb.AddForce(direction * _pushBackForce, ForceMode2D.Impulse);
        Invoke("RemovePushBack", _pushBackTime);
    }

    protected void RemovePushBack()
    {
        _hasControl = true;
        _rb.velocity = Vector2.zero; // test
    }

    protected virtual void HandleInteraction()
    {
        if (anchoredDialogue && _hasInteracted)
        {
            if (anchoredDialogue.CompareTag("Police1"))
            {
                anchoredDialogue.GetComponent<BuzztownPoliceScript>().Talk();
            }
            else
            {
                anchoredDialogue.GetComponent<DocksPolicemanScript>().Talk();
            }
            _hasInteracted = false;
        }
    }

}
