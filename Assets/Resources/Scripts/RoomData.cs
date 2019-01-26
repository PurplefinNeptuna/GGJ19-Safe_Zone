using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TeleporterData {
	public string teleporter;
	public string targetRoom;
	public string targetTeleporter;
}

[CreateAssetMenu(fileName = "Room", menuName = "Create New Room")]
public class RoomData : ScriptableObject {
	public GameObject Room;
	public List<TeleporterData> teleporters;
}
