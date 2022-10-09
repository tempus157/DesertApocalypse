using System.Linq;
using UnityEngine;

public class TempTileMap : MonoBehaviour {
    [SerializeField]
    private Tile walkableTilePrefab;

    [SerializeField]
    private Tile nonWalkableTilePrefab;

    [SerializeField]
    private Vector2Int mapSize;

    [SerializeField]
    private Vector2Int[] nonWalkableTiles;

    private Tile[,] tileMap;

    private void Start() {
        tileMap = new Tile[mapSize.x, mapSize.y];

        for (var y = 0; y < mapSize.y; y++) {
            for (var x = 0; x < mapSize.x; x++) {
                // create hexagonal grid
                var position = new Vector3(x * 1.5f, y * 1.5f, 0);
                if (x % 2 == 1) {
                    position.y -= 0.75f;
                }

                // check if tile is walkable
                var isWalkable = nonWalkableTiles
                    .All(nonWalkableTile => nonWalkableTile.x != x || nonWalkableTile.y != y);

                // create tile
                var tile = Instantiate(
                    isWalkable ? walkableTilePrefab : nonWalkableTilePrefab,
                    position,
                    Quaternion.identity,
                    transform
                );
                tile.name = $"Tile {x} {y}";
                tileMap[x, y] = tile;
            }
        }

        // set neighbours
        for (var y = 0; y < mapSize.y; y++) {
            for (var x = 0; x < mapSize.x; x++) {
                var tile = tileMap[x, y];
                if (!tile.IsWalkable) {
                    continue;
                }

                // add lower near tile
                if (y > 0) {
                    tile.NearTiles.Add(TileDirection.Lower, tileMap[x, y - 1]);
                }

                // add lower left near tile
                if (x % 2 == 0) {
                    if (x > 0) {
                        tile.NearTiles.Add(TileDirection.LowerLeft, tileMap[x - 1, y]);
                    }
                } else {
                    if (x > 0 && y > 0) {
                        tile.NearTiles.Add(TileDirection.LowerLeft, tileMap[x - 1, y - 1]);
                    }
                }

                // add lower right near tile
                if (x % 2 == 0) {
                    if (x < mapSize.x - 1) {
                        tile.NearTiles.Add(TileDirection.LowerRight, tileMap[x + 1, y]);
                    }
                } else {
                    if (x < mapSize.x - 1 && y > 0) {
                        tile.NearTiles.Add(TileDirection.LowerRight, tileMap[x + 1, y - 1]);
                    }
                }

                // add upper near tile
                if (y < mapSize.y - 1) {
                    tile.NearTiles.Add(TileDirection.Upper, tileMap[x, y + 1]);
                }

                // add upper left near tile
                if (x % 2 == 0) {
                    if (x > 0 && y < mapSize.y - 1) {
                        tile.NearTiles.Add(TileDirection.UpperLeft, tileMap[x - 1, y + 1]);
                    }
                } else {
                    if (x > 0) {
                        tile.NearTiles.Add(TileDirection.UpperLeft, tileMap[x - 1, y]);
                    }
                }

                // add upper right near tile
                if (x % 2 == 0) {
                    if (x < mapSize.x - 1 && y < mapSize.y - 1) {
                        tile.NearTiles.Add(TileDirection.UpperRight, tileMap[x + 1, y + 1]);
                    }
                } else {
                    if (x < mapSize.x - 1) {
                        tile.NearTiles.Add(TileDirection.UpperRight, tileMap[x + 1, y]);
                    }
                }
            }
        }
    }
}