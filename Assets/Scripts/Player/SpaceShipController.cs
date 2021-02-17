using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System;

public class SpaceShipController : MonoBehaviour, IGameplayInputListener
{
    const int MAX_WEAPON_COUNT = 10;
    const float MAX_TILT = 30f;
    const float TILT_FACTOR = 10f;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float drag = 1f;
    [Header("Search Objects")]
    [SerializeField] private Transform weaponsTransform;
    
    // Weapons
    private ShipWeapon[] weapons;
    private Action weaponShootEvent;
    private Action specialWeaponShootEvent;

    // Input
    PlayerInputValues playerInput;
    private Vector2 movementVector;
    private bool shootPressed;
    private bool specialShootPressed;
    private float rotateDir;

    // movement
    private Rigidbody _rb;
    private Vector3 velocity;
    private float rotationVelocity;


    // Public properties
    public PlayerInputValues Controller => playerInput;
    
    private void Awake() 
    {
        playerInput = new PlayerInputValues();
        _rb = GetComponent<Rigidbody>();

        if (weaponsTransform.childCount == 0) return;
        weapons = new ShipWeapon[MAX_WEAPON_COUNT];

        for (int i = 0; i < weapons.Length; i++)
        {
            if (i >= weaponsTransform.childCount) break;
            weapons[i] = weaponsTransform.GetChild(i).GetComponent<ShipWeapon>();
        }
    }

    private void OnEnable() 
    {
        playerInput.AssignInput(this);
        if (weapons == null) return;
        foreach(ShipWeapon weapon in weapons)
        {
            if (weapon == null) continue;

            if (weapon.SpecialWeapon)
            {
                specialWeaponShootEvent += weapon.Shoot;
            }
            else
            {
                weaponShootEvent += weapon.Shoot;
            }   
        }
    }
    private void OnDisable() 
    {
        playerInput.RemoveInput(this);
        if (weapons == null) return;
        foreach(ShipWeapon weapon in weapons)
        {
            if (weapon == null) continue;
            weapon.ship = GetComponent<Ship>();
            if (weapon.SpecialWeapon)
            {
                specialWeaponShootEvent += weapon.Shoot;
            }
            else
            {
                weaponShootEvent += weapon.Shoot;
            }  
        }
    }

    #region input
    public void OnMovement(InputAction.CallbackContext value)
    {
        movementVector = value.ReadValue<Vector2>();
        
        if (value.phase == InputActionPhase.Canceled)
        {
            movementVector = Vector2.zero;
        }
    }

    public void OnFire(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
            shootPressed = true;
        else
        {
            shootPressed = false;
        }
    }

    public void OnSpecialFire(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
            specialShootPressed = true;
        else
        {
            specialShootPressed = false;
        }
    }

    public void OnRotate(InputAction.CallbackContext value)
    {
        rotateDir = value.ReadValue<float>();
    }

    #endregion
    
    private void Update() 
    {
        // Get inputs and create an acceleration
        Vector2 inputAcceleration = movementVector * acceleration;

        rotationVelocity += rotateDir * Time.deltaTime * acceleration;
        
        if (rotateDir == 0)
        {
            velocity.z += inputAcceleration.y * Time.deltaTime;
            velocity.x += inputAcceleration.x * Time.deltaTime;
        }

        if (shootPressed)
        {
            weaponShootEvent?.Invoke();
        }
        if (specialShootPressed)
        {
            specialWeaponShootEvent?.Invoke();
        }
    }

    private void FixedUpdate() 
    {
        // Velocity Drag
        velocity = velocity * (1 - Time.deltaTime * drag);
        rotationVelocity = rotationVelocity * (1 - Time.deltaTime * drag);

        // Clamp velocity magnitude to stay between the given value
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Move the ship's position
        transform.position += velocity * Time.deltaTime;

        float tilt = 0f;

        
        if (Mathf.Abs(rotateDir) > 0)
        {
            Vector3 newDir = transform.forward;
            newDir = Quaternion.Euler(0f, rotateDir * 45f, 0f) * newDir;
            
            transform.forward = Vector3.Slerp
                (transform.forward,
                newDir,
                Time.deltaTime * 2f);
         
        }
        else if (movementVector.magnitude > 0.1f)
        {
            // Rotate ship thowards input
            transform.forward = Vector3.Slerp
                (transform.forward,
                velocity,
                Time.deltaTime * 2f);
            
            tilt = Mathf.Clamp(velocity.x * TILT_FACTOR, -MAX_TILT, MAX_TILT);
            transform.Rotate(0, 0, tilt);
        }
    }
}
