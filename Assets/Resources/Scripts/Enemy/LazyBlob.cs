using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyBlob : Enemy {

	private float attackDelayMax = 2f;
	private float attackDelay;

	public override void SetDefault() {
		health = 20;
		damage = 10;
		affectedByGravity = false;
		attackPower = 2f;
		attackDelay = Random.Range(attackDelayMax - 1f, attackDelayMax + 1f);
	}

	private Vector3 playerPos;
	private Vector3Int playerGridPos;
	private Vector3Int gridPos;
	private Vector2 face;

	public override void AI() {
		playerPos = targetPlayer.transform.position;
		playerGridPos = GameScript.main.grid.WorldToCell(playerPos);
		gridPos = GameScript.main.grid.WorldToCell(rb2d.position);
		Vector2 dist = (Vector2)playerPos - rb2d.position;
		if (dist.x < 0) {
			spriteRenderer.flipX = true;
			face = Vector2.left;
		}
		else {
			spriteRenderer.flipX = false;
			face = Vector2.right;
		}

		Vector3Int GridDist = playerGridPos - gridPos;
		if (Mathf.Abs(GridDist.y) <= 2)
			attackDelay -= Time.deltaTime;

		if (attackDelay <= 0f) {
			attackDelay = Random.Range(attackDelayMax - 1f, attackDelayMax + 1f);
			ProjectileManager.Spawn(rb2d.position, 10f, face, false, gameObject, damage, "BigBullet", "BigBullet");
		}
	}
}
