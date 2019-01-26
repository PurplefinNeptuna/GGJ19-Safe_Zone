using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PhysicsObject {

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
			velocity.y = jumpTakeOffSpd;
		}
		else if (Input.GetButtonUp("Jump")) {
			if (velocity.y > 0) {
				velocity.y *= .5f;
			}
		}

		bool flipSprite = (spriteRenderer.flipX ? (xMove > 0.01f) : (xMove < -0.01f));
		if (flipSprite && !inJumpBack)
			spriteRenderer.flipX = !spriteRenderer.flipX;

		velocity.x = xMove * maxSpeed;

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
				GameScript.main.lastY = rb2d.position.y;
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
