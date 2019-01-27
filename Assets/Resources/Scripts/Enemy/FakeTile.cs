using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTile : Enemy {

	public override void SetDefault() {
		health = 30;
		affectedByGravity = false;
		velocity = Vector2.zero;
		attackPower = 0f;
		damage = 0;
		noContactDamage = true;
		useCustomHit = true;
	}

public override void CustomGetHit(int damage, Player source) {
		if (source.gunLevel >= 1) {
			health -= damage;
		}
	}
}
