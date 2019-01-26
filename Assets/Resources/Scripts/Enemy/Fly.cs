using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Enemy {

	private float attackDelayMax = 2f;
	private float attackDelay = 2f;
	private float attackTimeMax = .5f;
	private float attackTime = .5f;
	private bool chasing = false;
	private bool attacking = false;
	private Vector2 playerDir;
	private float atkSpeed = 10f;

	public override void SetDefault() {
		affectedByGravity = false;
		velocity = Vector2.zero;
		attackPower = 1f;
		damage = 10;
	}

	public override void AI() {
		playerDir = targetPlayer.GetComponent<Player>().GetPosition() - rb2d.position;
		if (playerDir.sqrMagnitude <= 225f) {
			chasing = true;
		}
		else {
			chasing = false;
			attackDelay = attackDelayMax;
		}

		if (chasing && !attacking) {
			attackDelay -= Time.deltaTime;
			if (attackDelay <= 0f) {
				attackDelay = attackDelayMax;
				attacking = true;
				attackTime = attackTimeMax;
				Speed = atkSpeed;
				Direction = playerDir.normalized;
			}
		}
		else if (attacking) {
			attackTime -= Time.deltaTime;
			if (attackTime <= 0f) {
				velocity = Vector2.zero;
				attacking = false;
			}
		}
	}
}
