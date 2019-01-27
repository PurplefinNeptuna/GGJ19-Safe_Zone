using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperBlob : Enemy {

	private float attackDelayMax = 2f;
	private float attackDelay;

	public override void SetDefault() {
		health = 20;
		damage = 10;
		affectedByGravity = true;
		attackPower = 2f;
		attackDelay = Random.Range(attackDelayMax - 1f, attackDelayMax + 1f);
	}

	private Vector3 playerPos;
	private Vector2 face;

	public override void AI() {
		playerPos = targetPlayer.transform.position;
		if (grounded) {
			Vector2 dist = (Vector2)playerPos - rb2d.position;
			if (dist.x < 0) {
				spriteRenderer.flipX = true;
				face = Vector2.left;
			}
			else {
				spriteRenderer.flipX = false;
				face = Vector2.right;
			}
		}

		if (grounded) {
			attackDelay -= Time.deltaTime;
			velocity.x = 0;
		}
		else {
			velocity.x = 4f * face.x;
		}

		if (attackDelay <= 0f) {
			attackDelay = Random.Range(attackDelayMax - 1f, attackDelayMax + 1f);
			velocity.y = Random.Range(10f, 15f);
		}
	}
}
