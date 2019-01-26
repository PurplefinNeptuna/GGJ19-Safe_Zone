using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager {
	public static GameObject Spawn(Vector2 worldPos, string enemyName = "Spike", string behaviourName = "Spike") {
		Type type;
		GameObject prefab = Resources.Load<GameObject>("Prefabs/EnemyShapes/" + enemyName);
		try {
			type = Type.GetType(behaviourName);
		}
		catch {
			type = typeof(Spike);
		}
		GameObject result = GameObject.Instantiate<GameObject>(prefab, worldPos, Quaternion.identity);
		result.AddComponent(type);
		result.layer = LayerMask.NameToLayer("Enemy");
		Enemy enemy = result.GetComponent(type) as Enemy;
		enemy.targetPlayer = GameScript.main.player;
		return result;
	}
}
