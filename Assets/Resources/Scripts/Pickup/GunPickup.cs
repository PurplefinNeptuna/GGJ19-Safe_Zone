using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : Pickup {

	public int maxGunLevel;
	public int minGunLevel;

	public override void OnHitPlayer(Player player) {
		if (player.gunLevel >= minGunLevel && (maxGunLevel == 0 || player.canShoot)) {
			if (player.canShoot) {
				player.gunLevel = Math.Max(Math.Min(player.gunLevel + 1, maxGunLevel), player.gunLevel);
			}
			else {
				player.canShoot = true;
			}
			Destroy(gameObject);
		}
	}
}
