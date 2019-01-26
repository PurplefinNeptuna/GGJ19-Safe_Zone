using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Projectile {
	public override void SetDefault() {
		affectedByGravity = false;
		lifeTime = .5f;
		size = 0.1875f;
	}

	public override void OnHitGround() {
		Destroy(gameObject);
	}

	public override void OnHitEnemy(GameObject target) {
		target.GetComponent<Enemy>().GetHit(damage);
		Destroy(gameObject);
	}

	private void OnDestroy() {
		Source.GetComponent<Player>().gunAmmo--;
	}
}
