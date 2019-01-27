﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PhysicsObject {

	public bool godMode = false;
	public float maxSpeed = 6f;
	public float jumpTakeOffSpd = 13f;
	public float knockBackXSpeed = .5f;
	public float knockBackYSpeed = 4f;
	public float knockBackResistance = 1f;
	public float knockBackDirection = 1f;
	public float invincibleTimeMax = 1.5f;
	private float invincibleTime = 0f;
	public bool invincible = false;
	private bool blinking = false;
	public float blinkRateMax = .2f;
	private float blinkRate;
	public int maxHealth = 100;
	public int health = 100;

	public bool jumpBack = false;
	private bool inJumpBack = false;

	private SpriteRenderer spriteRenderer;

	public bool canShoot = false;
	public float gunDistance = .6f;
	public Vector2 GunDirection {
		get {
			return spriteRenderer.flipX ? Vector2.left : Vector2.right;
		}
	}
	public int gunDamage = 5;
	public bool gunFired = false;
	public float gunBulletSpeed = 10f;
	public int gunAmmoMax = 2;
	public int gunAmmo = 0;
	public int gunLevel = 0;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update() {
		if (health <= 0) {
			health = 0;
			Dead();
		}

		if (invincibleTime > 0f) {
			invincibleTime -= Time.deltaTime;
		}
		else {
			invincibleTime = 0f;
			invincible = false;
			blinking = false;
			spriteRenderer.color = Color.white;
		}

		float xMove = 0f;

		if (grounded && velocity.y <= 0f) {
			inJumpBack = false;
			jumpBack = false;
		}
		else if (jumpBack) {
			inJumpBack = true;
		}

		if (!inJumpBack || grounded)
			xMove = Input.GetAxis("Horizontal");
		else
			xMove = knockBackXSpeed * knockBackResistance * knockBackDirection;

		if (Input.GetButtonDown("Jump") && grounded) {
			velocity.y = jumpTakeOffSpd + (godMode ? 5f : 0f);
		}
		else if (Input.GetButtonUp("Jump")) {
			if (velocity.y > 0) {
				velocity.y *= .5f;
			}
		}

		float vertRaw = Input.GetAxisRaw("Vertical");
		Vector2 finalGunDirection = (Mathf.Abs(vertRaw) > .5f) ? Vector2.up * Mathf.Sign(vertRaw) : GunDirection;

		if (canShoot) {
			if (Input.GetButtonDown("Fire1") && !gunFired && gunAmmo < gunAmmoMax + gunLevel) {
				gunFired = true;
				int bulletDamage = Random.Range(gunDamage - 2, gunDamage + 3) + gunLevel * 2;
				bulletDamage *= (godMode ? 5 : 1);
				ProjectileManager.Spawn(rb2d.position + finalGunDirection * gunDistance, gunBulletSpeed + (gunLevel * 5f), finalGunDirection, true, gameObject, bulletDamage, "Bullet", "PlayerBullet");
				gunAmmo++;
			}
			else if (Input.GetButtonUp("Fire1") && gunFired) {
				gunFired = false;
			}
		}

		bool flipSprite = (spriteRenderer.flipX ? (xMove > 0.01f) : (xMove < -0.01f));
		if (flipSprite && !inJumpBack)
			spriteRenderer.flipX = !spriteRenderer.flipX;

		velocity.x = xMove * (maxSpeed + (godMode ? 5f : 0f));

		if (blinking) {
			if (blinkRate > blinkRateMax / 2f) {
				spriteRenderer.color = new Color(1, 1, 1, .5f);
			}
			else {
				spriteRenderer.color = Color.white;
			}
			blinkRate -= Time.deltaTime;
			if (blinkRate <= 0) {
				blinkRate = blinkRateMax;
			}
		}
	}

	public void Knockback(Vector2 direction, float attackMultiplier = 1f) {
		knockBackDirection = Mathf.Sign(direction.x);
		jumpBack = true;
		velocity.y = knockBackYSpeed * knockBackResistance * attackMultiplier;
	}

	public void GetHit(Vector2 direction, int damage, bool ignoreInvincible = false, float knockbackMultiplier = 1f) {
		if (godMode)
			return;
		if (!invincible || ignoreInvincible) {
			health -= damage;
			if (health < 0)
				health = 0;
			invincible = true;
			invincibleTime = invincibleTimeMax;
			blinking = true;
			blinkRate = blinkRateMax;
			Knockback(direction, knockbackMultiplier);
		}
	}

	public void Dead() {
		GameScript.main.GameOver();
		Destroy(gameObject);
	}
	private void OnTriggerStay2D(Collider2D collision) {
		Vector3Int pos = GameScript.main.grid.WorldToCell(rb2d.position);
		WorldTile teleporter = GameScript.main.teleporter.SingleOrDefault(x => x.localPlace == pos);
		if (teleporter != null) {
			if (teleporter.name[0] == 'I') {
				GameScript.main.Teleport(teleporter);
			}
		}
	}

	public void SetPosition(Vector3 pos) {
		rb2d.position = pos;
	}

	public Vector2 GetPosition() {
		return rb2d.position;
	}
}
