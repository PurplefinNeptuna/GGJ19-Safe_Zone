using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : Projectile {
	public override void SetDefault() {
		affectedByGravity = false;
		lifeTime = .5f;
		size = 0.1875f;
	}

	public override void OnHitGround() {
		Destroy(gameObject);
	}

	public override void OnHitEnemy(GameObject target) {
		target.GetComponent<Enemy>().GetHit(damage, Source.GetComponent<Player>());
		Destroy(gameObject);
	}

	public override void OnHitPlayer(GameObject target) {
		Vector3 playerPos = target.GetComponent<Transform>().position;
		target.GetComponent<Player>().GetHit(playerPos - transform.position, damage);
		Destroy(gameObject);
	}
}
