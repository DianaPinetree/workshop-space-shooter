using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapon : MonoBehaviour
{
    [Header("Physical Properties")]
    [SerializeField] protected Projectile bulletPrefab;
    [SerializeField] protected float bulletTravelDistance;
    [SerializeField] private Animator animator;

    [Header("Weapon Design Properties")]
    [SerializeField, Tooltip("Makes this weapon shoot with the special button instead of the normal shoot button")]
    private bool specialWeapon;
    [SerializeField, Range(.5f, 3f)] protected float attackSpeed;
    [SerializeField] protected int weaponDamage;
    [Header("Weapon Sound")]
    [SerializeField] private AudioClip shootSound;
    protected float attackSpeedCommulative;
    protected List<Projectile> liveProjectiles;
    public bool SpecialWeapon => specialWeapon;
    public Ship ship {get; set;} // ship this weapon belongs to
    private void Awake() 
    {
        liveProjectiles = new List<Projectile>();
        attackSpeedCommulative = attackSpeed;
        
        if (animator)
            animator.speed = 1 / attackSpeed + 1f;
    }
    public virtual void Shoot()
    {
        if (attackSpeedCommulative < attackSpeed) return;
        
        AudioManager.PlaySFX(shootSound, Random.Range(0.8f, 1f), Random.Range(0.85f, 1f));
        if (animator)
            animator.SetTrigger("Shoot");

        // Manage the pool of projectiles
        if (GetProjectile(out Projectile p))
        {
            p.transform.position = transform.position;
            p.transform.rotation = transform.rotation;
            p.gameObject.SetActive(true);
            
           Ship target = MatchManager.Ships[0] == ship? 
                MatchManager.Ships[1] : MatchManager.Ships[0]; 
            // Shoot
            p.Move(bulletTravelDistance, weaponDamage, target.transform); 
        }
        else
        {
            Projectile newProjectile = Instantiate(bulletPrefab,
                transform.position, 
                transform.rotation);
            
            liveProjectiles.Add(newProjectile);
            newProjectile.gameObject.SetActive(true);

            Ship target = MatchManager.Ships[0] == ship? 
                MatchManager.Ships[1] : MatchManager.Ships[0]; 
            // Shoot
            newProjectile.Move(bulletTravelDistance, weaponDamage, target.transform);
        }
        attackSpeedCommulative = 0;
    }

    private void Update() 
    {
        if (attackSpeedCommulative < attackSpeed)
        {
            attackSpeedCommulative += Time.deltaTime;
        }
    }

    private bool GetProjectile(out Projectile projectile)
    {
        if (liveProjectiles.Count <= 0) 
        {
            projectile = null;
            return false;
        }

        for (int i = liveProjectiles.Count - 1; i >= 0; i--)
        {
            if (!liveProjectiles[i].gameObject.activeSelf)
            {
                projectile = liveProjectiles[i];
                return true;
            }
        }

        projectile = null;
        return false;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, transform.forward * bulletTravelDistance);
    }
}
