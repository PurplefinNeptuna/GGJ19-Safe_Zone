using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {
	public float minGroundNormalY = .65f;
	public Vector2 currentNormal;
	public float gravityModifier = 1f;
	public bool affectedByGravity = true;
	public LayerMask mask;
	protected bool grounded;
	protected ContactFilter2D contactFilter;
	protected Rigidbody2D rb2d;
	protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
	public Vector2 velocity = Vector2.zero;
	protected Vector2 groundNormal = Vector2.up;
	protected const float minMoveDist = 0.001f;
	protected const float shellRadius = 0.01f;

	void OnEnable() {
		rb2d = GetComponent<Rigidbody2D>();
		POOnEnable();
	}

	void Start() {
		contactFilter.useTriggers = false;
		mask = Physics2D.GetLayerCollisionMask(gameObject.layer);
		contactFilter.SetLayerMask(mask);
		contactFilter.useLayerMask = true;
		POStart();
	}

	void FixedUpdate() {
		if (affectedByGravity)
			velocity += gravityModifier * Physics2D.gravity * Time.fixedDeltaTime;

		Vector2 deltaPos = velocity * Time.fixedDeltaTime;

		if (!(affectedByGravity || grounded))
			groundNormal = Vector2.up;
		Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
		grounded = false;

		Vector2 move = moveAlongGround * deltaPos.x;

		Movement(move, false);
		move = Vector2.up * deltaPos.y;
		Movement(move, true);
		POFixedUpdate();
	}

	void Movement(Vector2 move, bool yMovement) {
		float distance = move.magnitude;

		if (distance > minMoveDist) {
			int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

			hitBufferList.Clear();
			for (int i = 0; i < count; i++) {
				hitBufferList.Add(hitBuffer[i]);
			}

			for (int i = 0; i < hitBufferList.Count; i++) {
				int thisHitLayer = hitBufferList[i].collider.gameObject.layer;

				currentNormal = hitBufferList[i].normal;
				if (currentNormal.y > minGroundNormalY) {
					grounded = true;
					if (yMovement) {
						groundNormal = currentNormal;
						//currentNormal.x = 0;
					}
				}

				float projection = Vector2.Dot(velocity, currentNormal);
				if (projection < 0) {
					velocity = velocity - projection * currentNormal;
				}

				float modifiedDistance = hitBufferList[i].distance - shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
		}
		rb2d.position = rb2d.position + move.normalized * distance;
	}

	protected virtual void POFixedUpdate() {
	}

	protected virtual void POStart() {
	}

	protected virtual void POOnEnable() {
	}
}
