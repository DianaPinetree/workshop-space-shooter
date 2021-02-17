using UnityEngine.InputSystem;

public interface IGameplayInputListener
{
    void OnMovement(InputAction.CallbackContext value);
    void OnFire(InputAction.CallbackContext value);
    void OnRotate(InputAction.CallbackContext value);
    void OnSpecialFire(InputAction.CallbackContext value);
}