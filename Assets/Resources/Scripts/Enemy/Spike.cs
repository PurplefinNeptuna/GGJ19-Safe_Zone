using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Enemy {
	public override void SetDefault() {
		affectedByGravity = false;
		velocity = Vector2.zero;
		attackPower = 2f;
		damage = 15;
		immortal = true;
	}
}
