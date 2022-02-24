using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  [SerializeField] GameObject projectile;
  [Tooltip("Number of shots per seconds")]
  [SerializeField, Min(0.1f)] float rateOfFire;
  [SerializeField] int maxProjectileAtOnce = 1;
  [SerializeField] Transform launchPoint;
  [Tooltip("-1 is infinite")]
  [SerializeField] float ammo = -1;

  private float lastFireTime = float.MinValue;
  private int projectile_at_flight = 0;

  private void Start()
  {
    EventBus.Instance.Register("projectile-die", CountProjectiles);
  }
  private void OnDestroy()
  {
    EventBus.Instance.Remove("projectile-die", CountProjectiles);
  }
  private void CountProjectiles(object sender, System.EventArgs e)
  {
    projectile_at_flight--;
  }

  public void Fire()
  {
    var delay = 1f / rateOfFire;
    if (Time.time - lastFireTime > delay &&
        (ammo == -1 || ammo > 0) &&
        (projectile_at_flight < maxProjectileAtOnce))
    {
      lastFireTime = Time.time;
      projectile_at_flight++;
      if (ammo > 0)
        ammo--;
      var pr = Instantiate(projectile, launchPoint.position, Quaternion.identity);
    }
  }
}
