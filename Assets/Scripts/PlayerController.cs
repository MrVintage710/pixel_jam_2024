using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    public float speed = 0.2f;
    
    private Animator _animator;
    private PlayerInput _playerInput;
    

    #region Input Processing
    private Vector2 _movement;
    private Vector2 _lookPoint;  //Gamepad direction or mouse position on screen.
    
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
    #endregion

    // Start is called before the first frame update
    void Start() {
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _weaponRotator = transform.Find("WeaponRotator");
    }

    // Update is called once per frame
    void Update() {
        _animator.SetBool("is_running", _movement != Vector2.zero);
        GameManager.virtualPosition += _movement * (speed * Time.deltaTime);
        RotateCrab();
    }

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
    private PlayerWeapon activeWeapon;

    private void CompileWeaponComponents()
    {
        weaponComponents = new Dictionary<string, PlayerWeapon>();
        foreach(PlayerWeapon weapon in GetComponents<PlayerWeapon>())
        {
            weaponComponents.Add(weapon.WeaponName, weapon);
        }
    }
    #endregion
}
