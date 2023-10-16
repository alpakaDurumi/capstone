using UnityEngine;

public interface IArrowHittable
{
    void Hit(RaycastHit hit, Arrow arrow);
}
