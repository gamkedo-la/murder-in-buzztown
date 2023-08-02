using UnityEngine;
using UnityEngine.InputSystem;

public struct InputStatus
{
    public Vector2 Move;
    public bool JumpPushed;
    public bool JumpHeld;
    public bool DashPushed;
    public bool MeleePushed;
    public bool ShootPushed;
}

public class PlayerInput : MonoBehaviour
{

    public InputStatus inputStatus { get; private set; }

    private PlayerInputs _inputs;
    private InputAction _move, _jump, _dash, _melee, _shoot;

    private void Awake()
    {
        _inputs = new PlayerInputs();
        _move = _inputs.Player.Move;
        _jump = _inputs.Player.Jump;
        _dash = _inputs.Player.Dash;
        _melee = _inputs.Player.Melee;
        _shoot = _inputs.Player.Shoot;
    }

    private void OnEnable() => _inputs.Enable();

    private void OnDisable() => _inputs.Disable();


    private void Update()
    {
        inputStatus = new InputStatus
        {
            JumpPushed = _jump.WasPressedThisFrame(),
            JumpHeld = _jump.IsPressed(),
            DashPushed = _dash.WasPressedThisFrame(),
            MeleePushed = _melee.WasPressedThisFrame(),
            Move = _move.ReadValue<Vector2>(),
            ShootPushed = _shoot.WasPressedThisFrame(),
        };
    }
}
