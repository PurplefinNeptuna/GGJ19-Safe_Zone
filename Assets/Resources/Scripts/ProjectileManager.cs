using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager {

	public static int projectileCount = 0;

	public static GameObject Spawn(Vector2 worldPos, float speed, Vector2 direction, bool friendly, GameObject source, int damage = 5, string projectileName = "Bullet", string behaviourName = "DefaultBullet") {
		Type type;
		GameObject prefab = Resources.Load<GameObject>("Prefabs/Projectiles/" + projectileName);
		try {
			type = Type.GetType(behaviourName);
		}
		catch {
			type = typeof(DefaultBullet);
		}
		projectileCount++;
		GameObject result = GameObject.Instantiate<GameObject>(prefab, worldPos, Quaternion.identity);
		result.AddComponent(type);
		Projectile projectile = result.GetComponent(type) as Projectile;
		projectile.Source = source;
		projectile.Friendly = friendly;
		projectile.defaultSpeed = speed;
		projectile.Speed = speed;
		projectile.Direction = direction;
		projectile.damage = damage;
		return result;
	}
}
