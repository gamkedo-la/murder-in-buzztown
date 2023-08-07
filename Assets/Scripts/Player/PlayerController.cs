using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector] private Rigidbody2D _rb;
    [SerializeField] private CapsuleCollider2D _coll;
    private PlayerInput _input;
    private InputStatus _inputStatus;

    private Vector2 _internalSpeed;
    private Vector2 _externalSpeed;
    private int _currentFrame;
    private bool _hasControl = true;
    // external
    public event Action<bool, float> OnGroundedChange;
    public event Action<bool, Vector2> OnDashChange;
    public event Action<bool> OnJump; //boolean to differentiate wall jump
    public event Action OnAirJump;
    public event Action OnMelee;
    public event Action OnShoot;

    public Vector2 movement => _inputStatus.Move;
    public Vector2 speed => _internalSpeed;
    public Vector2 velocity => _rb.velocity;

    // Collisions
    private bool _applyFriction;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] float _groundDistance;
    // [SerializeField] Vector2 _wallCheckSize;
    private readonly RaycastHit2D[] _groundChecks = new RaycastHit2D[2];
    private readonly RaycastHit2D[] _ceilChecks = new RaycastHit2D[2];
    private int _frameLeftGrounded = int.MinValue;
    private bool _grounded;
    private Vector2 _skinWidth = new(0.02f, 0.02f);

    // Jumps

    private bool _hasJump;
    private bool _canJump;
    private bool _jumpInterrupted;
    private bool _canCoyote;
    private int _frameJump;
    private int _airJumpsLeft;

    // Dash 

    private bool _hasDash;
    private bool _canDash;
    private bool _isDashing;
    private int _dashFrame;
    private float _dashTimer;
    private Vector2 _dashVelocity;

    //constants
    private const int JUMP_POWER = 36;

    private const int JUMP_BUFFER = 5;
    private const int COYOTE_BUFFER = 5;
    private const int MAX_AIR_JUMPS = 1;
    private const float DASH_COOLDOWN = 2f;
    private const int DASH_VELOCITY = 40;
    private const int DASH_DURATION = 6;
    private const float DASH_HORIZONTAL_DECAY = .5f;
    private const float X_AXIS_DEADZONE = .1f;
    private const float Y_AXIS_DEADZONE = .1f;
    private const int GROUND_SPEED_DECAY = 60;
    private const int AIR_SPEED_DECAY = 30;
    private const float FRICTION_MULTIPLIER = 1.5f;
    private const int MAX_HORIZONTAL_SPEED = 12;
    private const int HORIZONTAL_ACCELERATION = 100;



    private bool canAirJump => !_grounded && _airJumpsLeft > 0;
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
        if (resetVelocity) _rb.velocity = Vector2.zero;
        _hasControl = false;
    }

    public void ReturnControl()
    {
        _internalSpeed = Vector2.zero;
        _hasControl = true;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        Physics2D.queriesStartInColliders = false;
    }

    private void Update()
    {
        _inputStatus = _input.inputStatus;

        if (_inputStatus.JumpPushed)
        {
            _hasJump = true;
            _frameJump = _currentFrame;
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
            // needs to attack
        }

        if (_inputStatus.ShootPushed)
        {
            // needs to attack
        }
    }

    private void FixedUpdate()
    {
        _currentFrame++;
        HandleCollisions();
        HandleJump();
        HandleDash();

        HandleMovement();
        ApplyMovement();
    }

    private void HandleCollisions()
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
            _grounded = true;
            ResetDash();
            ResetJump();
            //TODO: Check if invoking is truly necesary or just additional function in controller
            OnGroundedChange?.Invoke(true, Mathf.Abs(_internalSpeed.y));
            if (_inputStatus.Move.x == 0)
            {
                _applyFriction = true;
            }
        }
        else if (_grounded && groundHits == 0)
        {
            _grounded = false;
            _frameLeftGrounded = _currentFrame;
            OnGroundedChange?.Invoke(false, 0);
        }
    }

    private void HandleJump()
    {
        if (!_jumpInterrupted && !_grounded && !_inputStatus.JumpHeld && _rb.velocity.y > 0) _jumpInterrupted = true;

        bool hasStoredJump = _canJump && _currentFrame < _frameJump + JUMP_BUFFER;
        if (!_hasJump && !hasStoredJump) return;

        bool hasCoyote = _canCoyote && !_grounded && _currentFrame < _frameJump + COYOTE_BUFFER;

        if (_grounded || hasCoyote)
        {
            Jump();
        }

        if (_hasJump && !_grounded && _airJumpsLeft > 0)
        {
            AirJump();
        }
        _hasJump = false;
    }

    private void Jump()
    {
        _jumpInterrupted = false;
        _frameJump = 0;
        _canJump = false;
        _canCoyote = false;
        _internalSpeed.y = JUMP_POWER;
        OnJump?.Invoke(false);
    }

    private void AirJump()
    {
        _jumpInterrupted = false;
        _airJumpsLeft--;
        _internalSpeed.y = JUMP_POWER;
        _externalSpeed.y = 0;
        OnAirJump.Invoke();
    }

    private void ResetJump()
    {
        _canCoyote = true;
        _canJump = true;
        _jumpInterrupted = false;
        _airJumpsLeft = MAX_AIR_JUMPS;
    }

    private void HandleDash()
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

    private void ResetDash()
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

    private void HandleMovement()
    {

        if (_isDashing) return;
        Debug.Log(_inputStatus.Move);
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

    private void ApplyMovement()
    {
        if (!_hasControl) return;

        _rb.velocity = _internalSpeed + _externalSpeed;
        _externalSpeed = Vector2.MoveTowards(_externalSpeed, Vector2.zero, .2f * Time.fixedDeltaTime); //TODO determine external speed decay    
    }


}