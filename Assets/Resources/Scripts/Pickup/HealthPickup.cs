using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup {
	public override void OnHitPlayer(Player player) {
		player.health = Math.Min(player.health + 15, player.maxHealth);
		Destroy(gameObject);
	}
}
