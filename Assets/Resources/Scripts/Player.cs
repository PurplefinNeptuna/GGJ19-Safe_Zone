using UnityEngine;

public class Player : PhysicsObject {

	public float maxSpeed = 6f;
	public float jumpTakeOffSpd = 12f;
	public float knockBackXSpeed = .5f;
	public float knockBackYSpeed = 4f;
	public float knockBackMultiplier = 1f;
	public float knockBackCooldownMax = 2f;
	public float knockBackCooldown = 0f;
	public float knockBackDirection = 1f;

	public bool jumpBack = false;
	private bool inJumpBack = false;

	private SpriteRenderer spriteRenderer;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update() {
		if (knockBackCooldown > 0f) {
			knockBackCooldown -= Time.deltaTime;
		}
		else {
			knockBackCooldown = 0f;
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
			xMove = knockBackXSpeed * knockBackMultiplier * knockBackDirection;

		if (Input.GetButtonDown("Jump") && grounded) {
			velocity.y = jumpTakeOffSpd;
		}
		else if (Input.GetButtonUp("Jump")) {
			if (velocity.y > 0) {
				velocity.y *= .5f;
			}
		}

		if (Input.GetButtonDown("Fire2") && !jumpBack) {
			Knockback(spriteRenderer.flipX ? Vector2.right : Vector2.left);
		}

		bool flipSprite = (spriteRenderer.flipX ? (xMove > 0.01f) : (xMove < -0.01f));
		if (flipSprite && !inJumpBack)
			spriteRenderer.flipX = !spriteRenderer.flipX;

		velocity.x = xMove * maxSpeed;
	}

	public void Knockback(Vector2 direction, int attackMultiplier = 1) {
		if (knockBackCooldown == 0f) {
			knockBackDirection = Mathf.Sign(direction.x);
			jumpBack = true;
			velocity.y += knockBackYSpeed * knockBackMultiplier * attackMultiplier;
			knockBackCooldown = knockBackCooldownMax;
		}
	}
}
