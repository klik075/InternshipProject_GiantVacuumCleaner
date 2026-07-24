using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]
    public PlayerController _playerController;
    [SerializeField]
    private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        _playerController.OnRunEvent += Run;
        _playerController.OnWalkEvent += Walk;
    }
    private void OnDisable()
    {
        _playerController.OnRunEvent -= Run;
        _playerController.OnWalkEvent -= Walk;
    }
    private void Walk(Vector2 obj)
    {
        _animator.SetBool(IsRunning, false);
        _animator.SetBool(IsWalking, obj.magnitude > .5f);
    }
    private void Run(Vector2 obj)
    {
        _animator.SetBool(IsRunning, true);
    }

}
