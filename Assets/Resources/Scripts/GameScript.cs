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
	public GameObject player;
	public int score = 0;
	public bool dead = false;
	public GameObject VCamPlayer;
	public GameObject room;
	public GameObject healthDrop;
	public ContactFilter2D playerContact;
	public Grid grid;
	public Dictionary<Vector3Int, WorldTile> tiles;
	public List<WorldTile> teleporter;
	public Text hpText;
	public Text scoreText;
	public GameObject gameOverPanel;
	public GameObject winPanel;
	public LayerMask playerLayer;
	public LayerMask enemyLayer;
	public LayerMask groundLayer;

	private void Awake() {
		if (main == null) {
			main = this;
		}
		else if (main != this) {
			Destroy(gameObject);
		}
	}

	private void Start() {
		LoadRoom();
		playerLayer = LayerMask.GetMask("Player");
		enemyLayer = LayerMask.GetMask("Enemy", "EnemyMimic");
		groundLayer = LayerMask.GetMask("Ground");
		playerContact.useTriggers = false;
		playerContact.SetLayerMask(playerLayer);
	}

	private void Update() {
		if (!dead)
			hpText.text = "HP: " + player.GetComponent<Player>().health;
		else if (Input.GetButtonDown("Jump")) {
			SceneManager.LoadScene(0);
		}
	}

	public void LoadRoom() {
		room = Instantiate<GameObject>(RoomManager.main.activeRoomData.Room, Vector3.zero, Quaternion.identity);
		room.name = RoomManager.main.activeRoomData.name;

		grid = room.GetComponentInChildren<Grid>();
		GetRoomTiles();

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

		player.GetComponent<Player>().SetPosition(spawnTile.worldLocation);
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
		scoreText.text = "score: " + score;
		winPanel.SetActive(true);
		dead = true;
	}

	public void Teleport(WorldTile source) {
		if (source.roomTarget == room.name) {
			WorldTile target = teleporter.SingleOrDefault(x => x.name == source.teleportTarget);
			if (target != null) {
				player.GetComponent<Player>().SetPosition(target.worldLocation);
			}
		}
		else {
			RoomManager.main.activeRoomName = source.roomTarget;
			RoomManager.main.spawnFrom = source.teleportTarget;
			RoomManager.main.LoadRoomData();
			Destroy(room);
			LoadRoom();
		}
	}
}
