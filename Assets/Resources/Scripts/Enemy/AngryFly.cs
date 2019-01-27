using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryFly : Enemy {

	private float attackDelayMax = 2f;
	private float attackDelay;
	private float attackTimeMax = .5f;
	private float attackTime = .5f;
	private bool chasing = false;
	private bool attacking = false;
	private Vector2 playerDir;
	private float atkSpeed = 5f;
	private int burstCount;
	private float burstDelayMax = .2f;
	private float burstDelay = .0f;

	public override void SetDefault() {
		attackDelay = Random.Range(attackDelayMax - 1f, attackDelayMax + 1f);
		burstCount = Random.Range(1, 4);
		affectedByGravity = false;
		velocity = Vector2.zero;
		attackPower = 1f;
		damage = 10;
		noContactDamage = false;
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
				attackDelay = Random.Range(attackDelayMax - 1f, attackDelayMax + 1f);
				attacking = true;
				attackTime = attackTimeMax;
				Speed = Random.Range(atkSpeed - 2f, atkSpeed + 2f);
				Direction = playerDir.normalized;
			}
			burstDelay -= Time.deltaTime;
			if (burstDelay <= 0f) {
				if (burstCount > 0) {
					ProjectileManager.Spawn(rb2d.position, 10f, playerDir.normalized, false, gameObject);
					burstCount--;
				}
				burstDelay = burstDelayMax;
			}
		}
		else if (attacking) {
			attackTime -= Time.deltaTime;
			if (attackTime <= 0f) {
				velocity = Vector2.zero;
				attacking = false;
				burstCount = Random.Range(1, 4);
			}
		}
	}
}
