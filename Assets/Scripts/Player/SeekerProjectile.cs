using UnityEngine;

public class SeekerProjectile : Projectile
{
    [SerializeField, Range(0.4f, 1.5f), Tooltip("How much will the projectile steer towards the other player")] 
    private float seekAmount;
    [SerializeField] private float explosionRadius;
    [SerializeField] private DeathParticles explosionParticles;
    [SerializeField] private AudioClip explosionSound;

    protected override void Movement()
    {
        Vector3 direction = seekTarget.position - transform.position;

        direction.Normalize();
        float step = seekAmount * Time.deltaTime;
        transform.forward = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);

        transform.position += Time.deltaTime * velocity * transform.forward;
    }

    protected override void DamageTarget(Ship hit)
    {

    }

    public void Explode()
    {
        AudioManager.PlaySFX(explosionSound, Random.Range(0.8f, 1f), Random.Range(0.8f, 1f));
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach(Collider c in cols)
        {
            if (c.TryGetComponent<Ship>(out Ship s))
            {
                s.TakeHit(damage);
            }
        }

        if (explosionParticles)
        {
            DeathParticles p = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            p.SetStartSize(explosionRadius);
        }
    }

    protected override void OnDrawGizmos() {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}