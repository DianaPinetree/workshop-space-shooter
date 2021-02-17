using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ship : MonoBehaviour
{
    public static List<Ship> Ships = new List<Ship>(2);
    public static Ship Other(Ship current)
    {
        return Ships[0] == current ? Ships[0] : Ships[1];
    }
    public const int MAX_HP = 100;
    const float COLLSISION_TIMER = 1.2f;
     
    [Header("Ship Visuals")]
    [SerializeField] private HitIndicator damageIndicator;
    [SerializeField] private DeathParticles deathParticles;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hitSound;

    private int currentHP;
    private float collisionDamageTimer;
    
    public int CurrentHP
    {
        get 
        {
            return currentHP;
        }
        private set
        {
            currentHP = value;
            hpChange?.Invoke();
        }
    }

    private void Awake() 
    {
        Ships.Add(this);
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<SphereCollider>();
        }

        Init();
    }

    public void Init()
    {
        currentHP = MAX_HP;
    }

    public void TakeHit(int damage)
    {
        // Change the current HP of the player
        CurrentHP -= damage;

        AudioManager.PlaySFX(hitSound, 1, 1);
        
        // Brief flash when taking damage
        if (damageIndicator)
            damageIndicator.Hit();

        // Check if the player is dead
        if (Dead())
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        AudioManager.PlaySFX(deathSound, 1, 1);

        gameObject.SetActive(false);
        if (deathParticles)
            Instantiate(deathParticles, transform.position, Quaternion.identity);
        if (damageIndicator)
            damageIndicator.Disable();
        deathEvent?.Invoke(this);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.TryGetComponent<Ship>(out Ship ship))
        {
            TakeHit(MAX_HP);
            ship.TakeHit(MAX_HP);
        }
        else
        {
            TakeHit((int)((float)MAX_HP * 0.7f));
        }
    }

    private bool Dead()
    {
        return CurrentHP < 0;
    }

    public static event Action<Ship> deathEvent;
    public static event Action hpChange;
}
