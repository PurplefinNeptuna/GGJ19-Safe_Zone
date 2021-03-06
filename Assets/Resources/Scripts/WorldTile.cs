﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class WorldTile {
	public Vector3Int localPlace;

	public Vector3 worldLocation;

	public TileBase tileBase;

	public Tilemap tilemapMember;

	public string name;

	public string roomTarget;

	public string teleportTarget;
}
