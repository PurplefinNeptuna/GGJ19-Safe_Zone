using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	public float size;
	public int damage;
	public float gravityModifier = 1f;
	public bool affectedByGravity = true;

	private bool _friendly;
	public LayerMask target;

	public bool Friendly {
		get {
			return _friendly;
		}
		set {
			_friendly = value;
			if (value) {
				target = GameScript.main.enemyLayer;
				gameObject.layer = LayerMask.NameToLayer("Projectile");
			}
			else {
				target = GameScript.main.playerLayer;
				gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
			}
		}
	}

	public bool sourceXFlip = false;
	public Vector2 spawnerPos;
	private GameObject _source;
	public GameObject Source {
		get {
			return _source;
		}
		set {
			_source = value;
			sourceXFlip = value.GetComponent<SpriteRenderer>().flipX;
			spawnerPos = value.transform.position;
		}
	}
	public bool stayAlive = false;
	public float lifeTime;

	protected SpriteRenderer spriteRenderer;

	public Vector2 velocity;
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

	public float defaultSpeed;
	private Collider2D[] colliderHitBuffer = new Collider2D[16];

	public float Speed {
		get {
			return velocity.magnitude;
		}
		set {
			if (velocity.normalized == Vector2.zero)
				velocity = value * Vector2.up;
			else
				velocity = value * velocity.normalized;
		}
	}
	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		SetDefault();
	}

	private void Update() {
		if (!stayAlive) {
			lifeTime -= Time.deltaTime;
			if (lifeTime <= 0f)
				Destroy(gameObject);
		}
		AI();
	}

	private void FixedUpdate() {
		if (affectedByGravity)
			velocity += gravityModifier * Physics2D.gravity * Time.fixedDeltaTime;

		Vector2 deltaPos = velocity * Time.fixedDeltaTime;

		transform.position += (Vector3)deltaPos;

		int count = Physics2D.OverlapCircleNonAlloc(transform.position, size, colliderHitBuffer, target);
		for (int i = 0; i < count; i++) {
			if (Friendly) {
				OnHitEnemy(colliderHitBuffer[i].gameObject);
			}
			else {
				OnHitPlayer(colliderHitBuffer[i].gameObject);
			}
		}

		Collider2D ground = Physics2D.OverlapCircle(transform.position, size, GameScript.main.groundLayer);
		if (ground != null) {
			OnHitGround();
		}
	}

	public virtual void SetDefault() {
		
	}
	public virtual void AI() {

	}
	public virtual void OnHitEnemy(GameObject target) {
	}

	public virtual void OnHitPlayer(GameObject target) {
	}

	public virtual void OnHitGround() {
	}
}
