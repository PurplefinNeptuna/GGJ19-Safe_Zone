﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PhysicsObject {
	public GameObject targetPlayer;
	public float defaultSpeed;
	public float attackPower;
	public int health;
	public bool immortal;
	public int damage;
	public bool dropHeart;
	public bool noContactDamage;
	public int score;
	protected bool useCustomHit;

	public float Speed {
		get {
			return velocity.magnitude;
		}
		set {
			defaultSpeed = value;
			if (velocity.normalized == Vector2.zero)
				velocity = value * Vector2.up;
			else
				velocity = value * velocity.normalized;
		}
	}

	public Vector2 Direction {
		get {
			return velocity.normalized;
		}
		set {
			if (velocity.magnitude == 0f)
				velocity = 1 * value;
			else
				velocity = velocity.magnitude * value;
		}
	}

	protected SpriteRenderer spriteRenderer;
	private Collider2D[] overlapPlayer = new Collider2D[4];

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (targetPlayer == null) {
			targetPlayer = GameScript.main.player;
		}
		SetDefault();
	}

	protected override void POFixedUpdate() {
		if (!GameScript.main.dead && !noContactDamage) {
			int count = rb2d.OverlapCollider(GameScript.main.playerContact, overlapPlayer);
			if (count > 0) {
				OnHitPlayer(targetPlayer);
				Player player = targetPlayer.GetComponent<Player>();
				Vector2 dir = player.GetPosition() - rb2d.position;
				player.GetHit(dir, damage, false, attackPower);
			}
		}
	}

	private void Update() {
		if (health <= 0 && !immortal) {
			Destroy(gameObject);
		}
		if (!GameScript.main.dead)
			AI();

		bool flipSprite = (spriteRenderer.flipX ? (velocity.x > 0.01f) : (velocity.x < -0.01f));
		if (flipSprite)
			spriteRenderer.flipX = !spriteRenderer.flipX;
	}

	public void GetHit(int damage, Player source) {
		if (!immortal) {
			if (useCustomHit)
				CustomGetHit(damage, source);
			else
				health -= damage;
		}
	}

	public virtual void AI() {
	}

	public virtual void SetDefault() {
	}

	public virtual void OnHitPlayer(GameObject player) {
	}

	public virtual void CustomGetHit(int damage, Player source) {
	}

	private void OnDestroy() {
		if (health <= 0) {
			if (dropHeart) {
				float chance = Random.Range(0f, 1f);
				if (chance * 3 <= 1f) {
					Instantiate<GameObject>(GameScript.main.healthDrop, rb2d.position, Quaternion.identity);
				}
			}
			GameScript.main.score += score;
		}
	}
}
