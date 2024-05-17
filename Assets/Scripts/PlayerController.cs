using System.Collections.Generic;
using System;
using Enemy;
using Unity.Entities.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {
    
    public static Vector2 Hitbox = new Vector2(16.0f, 16.0f);

    public float speed = 0.5f;
    public int health = 8;
    
    private Animator _animator;
    private PlayerInput _playerInput;
    private SpriteRenderer _spriteRenderer;
    private float _invulnerableTime;

    private void OnEnable() {
        ECSManager.DamagePlayerEventHandler += OnDamageEventHandler;
    }

    private void OnDamageEventHandler(object sender, DamagePlayerEvent e) {
        if (_invulnerableTime <= 0.0f) {
            Debug.Log("TAKING DAMAGE " + sender.GetType());
            health -= e.damage;
            _invulnerableTime = 1.0f;
            
            if (sender is InternalCompilerInterface.UncheckedRefRO<EnemyComponent> component) {
                Debug.Log(component.ValueRO.virtualPos);
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        
        _playerInput = GetComponent<PlayerInput>();


        _lookTransform = transform.Find("LookTransform");
        _animator = _lookTransform.GetComponentInChildren<Animator>();
        _spriteRenderer = _lookTransform.GetComponentInChildren<SpriteRenderer>();
        CompileWeaponComponents();
        _activeWeapon = weaponComponents["Claw"];
    }

    // Update is called once per frame
    void Update() {
        _animator.SetBool("is_running", _movement != Vector2.zero);
        transform.position += (Vector3)(_movement * (speed * Time.deltaTime));
        GameManager.playerPosition = transform.position;
        RotateCrab();
        ProcessWeaponTrigger();
        _invulnerableTime = Mathf.Clamp(_invulnerableTime - Time.deltaTime, 0.0f, 3.0f);
        if (_invulnerableTime > 0.0f) {
            _spriteRenderer.color = _spriteRenderer.color.WithAlpha(0.5f);
        } else {
            _spriteRenderer.color = _spriteRenderer.color.WithAlpha(1.0f);
        }
    }

    #region Input Processing
    private Vector2 _movement;
    private Vector2 _lookVector;  //Gamepad direction or mouse position on screen.
    private bool _weaponTrigger;
    
    public void OnMove(InputValue value) {
        _movement = value.Get<Vector2>();
    }

    public void OnLook(InputValue value) {
        if(_playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            _lookVector = (Vector2)(Camera.main.ScreenToWorldPoint(value.Get<Vector2>()) - transform.position);
        } else
        {
            _lookVector = value.Get<Vector2>();
        }
    }

    public void OnFire()
    {
        _weaponTrigger = true;
    }
    #endregion

    #region Rotation
    [SerializeField] private Transform cursorIndicator;

    [SerializeField] private float rotationRate = 4f; //In radians/sec
    private Transform _lookTransform; //Later we will probably just rotate the crab?
    private void RotateCrab()
    {
        Vector3 lookPoint = transform.position + (Vector3)_lookVector;
        cursorIndicator.position = lookPoint;

        Vector3 frameLookDirection = Vector3.RotateTowards(_lookTransform.forward, _lookVector, rotationRate * Time.deltaTime, 0);
        _lookTransform.LookAt(frameLookDirection + transform.position, Vector3.back); //World Up is -z, aka Vector3.back.
    }
    #endregion

    #region Weapon Management
    private Dictionary<string, PlayerWeapon> weaponComponents;
    private PlayerWeapon _activeWeapon;

    private void CompileWeaponComponents()
    {
        weaponComponents = new Dictionary<string, PlayerWeapon>();
        foreach(PlayerWeapon weapon in GetComponents<PlayerWeapon>())
        {
            weaponComponents.Add(weapon.WeaponName, weapon);
        }
    }
    private void ProcessWeaponTrigger()
    {
        if (_weaponTrigger)
        {
            _activeWeapon.PullTrigger();
            _weaponTrigger = false;
        }
    }
    #endregion
}
