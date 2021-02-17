using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputValues
{
    private GameInputs inputActions;
    private static int controllerIndex;
    public bool Assigned {get; private set;}
    public PlayerInputValues()
    {
        inputActions = new GameInputs();
    }

    public void Init()
    {
        if (Gamepad.all.Count > controllerIndex) // 0 > 0, 1 > 0, 1 > 1, 
        {
            Debug.Log(Gamepad.all[controllerIndex].name);
            inputActions.devices = new[] { Gamepad.all[controllerIndex] };
            Assigned = true;
            controllerIndex++;
            inputActions.Enable();
        }
    }

    public void AssignController(InputDevice device)
    {
        inputActions.devices = new[] {device};
        Assigned = true;
        Debug.Log("Player assigned with: " + device.name);
        inputActions.Enable();
    }

    public void AssignInput(IGameplayInputListener listener)
    {
        inputActions.Gameplay.Move.performed += listener.OnMovement;
        inputActions.Gameplay.Move.canceled += listener.OnMovement;
        inputActions.Gameplay.Shoot.performed += listener.OnFire;
        inputActions.Gameplay.Shoot.canceled += listener.OnFire;
        inputActions.Gameplay.SpecialShoot.performed += listener.OnSpecialFire;
        inputActions.Gameplay.SpecialShoot.canceled += listener.OnSpecialFire;
        inputActions.Gameplay.Rotate.performed += listener.OnRotate;
        inputActions.Gameplay.Rotate.canceled += listener.OnRotate;
    }

    public void RemoveInput(IGameplayInputListener listener)
    {
        inputActions.Gameplay.Move.performed -= listener.OnMovement;
        inputActions.Gameplay.Move.canceled -= listener.OnMovement;
        inputActions.Gameplay.Shoot.performed -= listener.OnFire;
        inputActions.Gameplay.Shoot.canceled -= listener.OnFire;
        inputActions.Gameplay.SpecialShoot.performed += listener.OnSpecialFire;
        inputActions.Gameplay.SpecialShoot.canceled += listener.OnSpecialFire;
        inputActions.Gameplay.Rotate.performed -= listener.OnRotate;
        inputActions.Gameplay.Rotate.canceled -= listener.OnRotate;
    }
}