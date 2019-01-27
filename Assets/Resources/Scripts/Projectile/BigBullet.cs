using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBullet : Projectile {
	public override void SetDefault() {
		affectedByGravity = false;
		lifeTime = 2f;
		size = 0.3125f;
	}

	public override void OnHitGround() {
		Destroy(gameObject);
	}

	public override void OnHitPlayer(GameObject target) {
		Vector3 playerPos = target.GetComponent<Transform>().position;
		target.GetComponent<Player>().GetHit(playerPos - transform.position, damage, false, 2);
		Destroy(gameObject);
	}
}
