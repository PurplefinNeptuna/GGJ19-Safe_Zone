using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : PhysicsObject {

	private Collider2D[] overlapPlayer = new Collider2D[4];


	protected override void POFixedUpdate() {
		if (!GameScript.main.dead) {
			int count = rb2d.OverlapCollider(GameScript.main.playerContact, overlapPlayer);
			if (count > 0) {
				OnHitPlayer(GameScript.main.player.GetComponent<Player>());
			}
		}
	}

	public virtual void OnHitPlayer(Player player) {
	}
}
