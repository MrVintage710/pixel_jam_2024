using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {
    public static event EventHandler<DamageEvent> DamageEventHandler;
    public static Vector2 Hitbox = new Vector2(16.0f, 16.0f);

    public float speed = 0.5f;
    public int health = 8;
    
    private Animator _animator;
    private PlayerInput _playerInput;
    private SpriteRenderer _spriteRenderer;
    private float _invulnerableTime;

    public struct DamageEvent {
        public int damage;
    }

    private void OnEnable() {
        DamageEventHandler += OnDamageEventHandler;
    }

    private void OnDamageEventHandler(object sender, DamageEvent e) {
        if (_invulnerableTime <= 0.0f) {
            Debug.Log("TAKING DAMAGE " + sender.GetType());
            health -= e.damage;
            _invulnerableTime = 1.0f;
            
            if (sender is InternalCompilerInterface.UncheckedRefRO<EnemyComponent> component) {
                Debug.Log(component.ValueRO.virtualPos);
            }
        }
    }

    public static void Damage(object source, int damage) {
        DamageEventHandler.Invoke(source, new DamageEvent { damage = damage});
    }

    // Start is called before the first frame update
    void Start() {
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _weaponRotator = transform.Find("WeaponRotator");
        CompileWeaponComponents();
        _activeWeapon = weaponComponents["Claw"];
    }

    // Update is called once per frame
    void Update() {
        _animator.SetBool("is_running", _movement != Vector2.zero);
        GameManager.virtualPosition += _movement * (speed * Time.deltaTime);
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
    private Vector2 _lookPoint;  //Gamepad direction or mouse position on screen.
    private bool _weaponTrigger;
    
    public void OnMove(InputValue value) {
        _movement = value.Get<Vector2>();
    }

    public void OnLook(InputValue value) {
        if(_playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            _lookPoint = (Vector2)Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        } else
        {
            _lookPoint = value.Get<Vector2>();
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
    private Transform _weaponRotator; //Later we will probably just rotate the crab?
    private void RotateCrab()
    {
        cursorIndicator.position = _lookPoint;
        
        Vector3 frameLookDirection = Vector3.RotateTowards(_weaponRotator.forward, _lookPoint, rotationRate * Time.deltaTime, 0);
        _weaponRotator.LookAt(frameLookDirection, Vector3.back); //World Up is -z, aka Vector3.back.
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
