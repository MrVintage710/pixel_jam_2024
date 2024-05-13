using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    public float speed = 0.2f;
    
    private Animator _animator;
    private PlayerInput _playerInput;
    private Vector2 _movement;

    public void OnMove(InputValue value) {
        _movement = value.Get<Vector2>();
    }

    // Start is called before the first frame update
    void Start() {
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update() {
        _animator.SetBool("is_running", _movement != Vector2.zero);
        GameManager.virtualPosition += _movement * (speed * Time.deltaTime);
    }
}
