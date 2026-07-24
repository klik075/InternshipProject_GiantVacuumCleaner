using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader",menuName = "Input/Reader")]
public class InputReader : DescriptionSO,GameInput.IGameplayActions
{
    #region Fields

    public event Action<Vector2> OnMovement = delegate { };

    private GameInput _gameInput;

    #endregion

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();

            _gameInput.Gameplay.SetCallbacks(this);//Gmaeplay 맵에 등록

            _gameInput.Gameplay.Enable();//활성화
        }
    }
    private void OnDisable()
    {
        if (_gameInput != null)
        {
            _gameInput.Gameplay.Disable();
            _gameInput.Gameplay.SetCallbacks(null);
        }
    }
    void GameInput.IGameplayActions.OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnMovement.Invoke(context.ReadValue<Vector2>());
        }
        else if (context.canceled)
        {
            OnMovement.Invoke(Vector2.zero);
        }
    }

    void GameInput.IGameplayActions.OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        { 
            Managers.ThingManager.Attack();
        }
    }
}
