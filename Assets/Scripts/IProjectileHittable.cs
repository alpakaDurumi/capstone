using UnityEngine;

public interface IProjectileHittable
{
    void Hit(RaycastHit hit, Projectile projectile);
}
