using UnityEngine;

namespace KienChi
{
	public class BulletSpawn : ObjectPool
	{
		public void SpawnBullet(Transform shooterDis, Block block, Shooter shooter)
		{
			GameObject bulletObject = GetPooledObject()?.gameObject;
			// Transform target = shooter.ShooterDisplay.BulletTranform;

			bulletObject?.transform.SetPositionAndRotation(shooterDis.position, shooterDis.rotation);
			bulletObject?.transform.SetParent(null);
			bulletObject?.SetActive(true);

			Bullet bullet = bulletObject.GetComponent<Bullet>();
			bullet.Shot(block, shooterDis, shooter);
		}

	}
}
