using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float velocity;
    [SerializeField, Tooltip("What layer this projectile SHOULDN'T collide with")] 
    private LayerMask collisionMask;
    private float distance;
    private float currentDistance = 0;
    private bool moving;
    protected int damage;
    protected Transform seekTarget;

    protected virtual void Update()
    {
        
    }

    protected virtual void Movement()
    {
        Vector3 deltaPosition = Time.deltaTime * velocity * transform.forward;
        transform.position += deltaPosition;
    }

    protected virtual void FixedUpdate()
    {
        if (moving)
        {
            Movement();

            currentDistance += Time.deltaTime * velocity;
            if (currentDistance > distance)
            {
                moving = false;
                OnDeath();
            }
        }

        if (Physics.Linecast(transform.position, transform.position + Time.deltaTime * velocity * transform.forward,
            out RaycastHit info, ~collisionMask))
        {
            if (info.transform.TryGetComponent<Ship>(out Ship ship))
            {
                DamageTarget(ship);
            }

            Debug.Log("Collision with: " + info.transform.gameObject);

            OnDeath();
        }
    }

    protected virtual void DamageTarget(Ship hit)
    {
        hit.TakeHit(damage);
    }

    public virtual void Move(float dst, int weaponDamage, Transform target = null)
    {
        moving = true;
        distance = dst;
        damage = weaponDamage;
        seekTarget = target;
    }

    protected virtual void OnDeath()
    {
        currentDistance = 0;
        gameObject.SetActive(false);
        projectileDeath?.Invoke();
    }

    public UnityEngine.Events.UnityEvent projectileDeath;

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawLine(transform.position, transform.position + Time.deltaTime * velocity * transform.forward);
    }
}
