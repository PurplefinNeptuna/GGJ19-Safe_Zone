using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
	public static RoomManager main;
	public List<RoomData> rooms;
	public string activeRoomName = "Room0";
	public RoomData activeRoomData;
	public string spawnFrom = "O0";

	private void Awake() {
		if (main == null) {
			main = this;
		}
		else if (main != this) {
			Destroy(gameObject);
		}

		rooms = Resources.LoadAll<RoomData>("RoomsData").ToList();

		if (activeRoomName == null || activeRoomName == "") {
			activeRoomName = rooms[0].name;
		}

		if (spawnFrom == null || spawnFrom == "") {
			spawnFrom = "O0";
		}

		LoadRoomData();
	}

	public void LoadRoomData() {
		activeRoomData = rooms.SingleOrDefault(x => x.name == activeRoomName);
		if (activeRoomData == null) {
			activeRoomData = rooms[0];
		}
	}
}
