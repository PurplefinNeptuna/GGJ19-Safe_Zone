using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameScript : MonoBehaviour {
	public static GameScript main;
	public bool exampleTest;
	public GameObject player;
	public bool dead = false;
	public GameObject VCamPlayer;
	public GameObject room;
	public ContactFilter2D playerContact;
	public Grid grid;
	public Dictionary<Vector3Int, WorldTile> tiles;
	public List<WorldTile> teleporter;
	public Text hpText;
	public Text scoreText;
	public GameObject gameOverPanel;
	public GameObject winPanel;

	private void Awake() {
		if (main == null) {
			main = this;
		}
		else if (main != this) {
			Destroy(gameObject);
		}

		LoadRoom();
	}

	private void Start() {
		playerContact.useTriggers = false;
		playerContact.SetLayerMask(LayerMask.GetMask("Player"));
	}

	private void Update() {
		if (!dead)
			hpText.text = "HP: " + player.GetComponent<Player>().health;
		else if (Input.GetButtonDown("Jump")) {
			SceneManager.LoadScene(0);
		}
	}

	public void LoadRoom() {
		if (!exampleTest)
			room = Instantiate<GameObject>(RoomManager.main.activeRoomData.Room, Vector3.zero, Quaternion.identity);

		grid = room.GetComponentInChildren<Grid>();
		GetRoomTiles();

		if (!exampleTest)
			SpawnPlayer();

		SetCamera();
	}

	private void GetRoomTiles() {
		tiles = new Dictionary<Vector3Int, WorldTile>();
		teleporter = new List<WorldTile>();
		foreach (Tilemap map in grid.GetComponentsInChildren<Tilemap>()) {
			foreach (Vector3Int pos in map.cellBounds.allPositionsWithin) {
				Vector3Int localplace = new Vector3Int(pos.x, pos.y, pos.z);

				if (!map.HasTile(localplace))
					continue;
				if (tiles.ContainsKey(localplace))
					tiles.Remove(localplace);
				WorldTile tile = new WorldTile() {
					localPlace = localplace,
					worldLocation = map.GetCellCenterWorld(localplace),
					tileBase = map.GetTile(localplace),
					tilemapMember = map,
				};

				tile.name = tile.tileBase.name;

				if (map.name == "Teleporter") {
					if (tile.name[0] == 'I' && tile.name.Length == 2 && char.IsDigit(tile.name[1])) {
						TeleporterData teleporter = RoomManager.main.activeRoomData.teleporters.SingleOrDefault(x => x.teleporter == tile.name);
						if (teleporter != null) {
							tile.roomTarget = teleporter.targetRoom;
							tile.teleportTarget = teleporter.targetTeleporter;
						}
					}
					teleporter.Add(tile);
				}

				tiles.Add(tile.localPlace, tile);
			}
		}
	}

	private void SpawnPlayer() {
		WorldTile spawnTile = teleporter.SingleOrDefault(x => x.name == RoomManager.main.spawnFrom);
		if (spawnTile == null)
			return;

		player.transform.position = spawnTile.worldLocation;
	}

	private void SetCamera() {
		CinemachineConfiner confiner = VCamPlayer.GetComponent<CinemachineConfiner>();
		confiner.m_BoundingShape2D = room.transform.GetChild(0).GetComponent<CompositeCollider2D>();
	}

	public void GameOver() {
		gameOverPanel.SetActive(true);
		dead = true;
	}

	public void Win() {
		winPanel.SetActive(true);
		dead = true;
	}
}
