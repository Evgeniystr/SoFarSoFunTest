using SoFarSoFun.Data;
using SoFarSoFun.ViewModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace SoFarSoFun.Services
{
    public class GameService : MonoBehaviour
    {
        [Inject(Id = Constants.Floor)]
        private Transform _floor;
        [Inject(Id = Constants.BallsParent)]
        private Transform _ballsParent;
        [Inject]
        private BallViewModel _ballPrefab;
        [Inject]
        private GameConfig _gameConfig;
        [Inject]
        private BallMaterialsService _ballMaterialsService;
        [Inject]
        private ScoreService _scoreService;

        private const float _scaleToSizeCoof = 10f;
        private const float _ballSize = 1f;
        private const float _ballYpos = _ballSize / 2;

        public void StartNewGame()
        {
            var ballsCount = _gameConfig.BallsCount;

            var floorSize = new Vector2(_floor.localScale.x * _scaleToSizeCoof, _floor.localScale.z * _scaleToSizeCoof);

            var spawnCellsGrid = new Vector2Int((int)(floorSize.x / _ballSize), (int)(floorSize.x / _ballSize));

            var maxBallsAvaliable = spawnCellsGrid.x * spawnCellsGrid.y;

            if (ballsCount > maxBallsAvaliable)
                ballsCount = maxBallsAvaliable;

            var ocupiedPositions = new List<Vector2Int>(ballsCount);

            for (int i = 0; i < ballsCount; i++)
            {
                var ballVM = Instantiate(_ballPrefab, _ballsParent);

                var spawnPosition = LookFreeSpawnPoint(ocupiedPositions, spawnCellsGrid, floorSize);
                ballVM.Initialize(spawnPosition, _ballMaterialsService, _scoreService, _gameConfig);
            }
        }

        private Vector3 LookFreeSpawnPoint(List<Vector2Int> ocupiedCells, Vector2Int spawnCellsGridSize, Vector2 floorSize)
        {
            Vector2Int cellToSpawn = Vector2Int.zero;


            var firstCellAssumption = new Vector2Int(Random.Range(0, spawnCellsGridSize.x), Random.Range(0, spawnCellsGridSize.y));

            if (IsCellFree(ocupiedCells, firstCellAssumption))
            {
                for (int h = 0; h < spawnCellsGridSize.x; h++)
                {
                    for (int w = 0; w < spawnCellsGridSize.x; w++)
                    {
                        var assumptionXcell = firstCellAssumption.x + w;
                        var assumptionYcell = firstCellAssumption.y + h;

                        if (assumptionXcell >= spawnCellsGridSize.x)
                            assumptionXcell -= spawnCellsGridSize.x;

                        if (assumptionYcell >= spawnCellsGridSize.y)
                            assumptionYcell -= spawnCellsGridSize.y;

                        var cellAssumption = new Vector2Int(assumptionXcell, assumptionYcell);

                        if (IsCellFree(ocupiedCells, cellAssumption))
                            continue;

                        cellToSpawn = cellAssumption;
                    }
                }
            }
            else
            {
                cellToSpawn = firstCellAssumption;
            }

            ocupiedCells.Add(cellToSpawn);

            //cell to pos

            var xSpawnPos = cellToSpawn.x * _ballSize - floorSize.x / 2 + _ballSize / 2;
            var zSpawnPos = cellToSpawn.y * _ballSize - floorSize.y / 2 + _ballSize / 2;
            var spawnPos = new Vector3(xSpawnPos, _ballYpos, zSpawnPos);

            return spawnPos;
        }

        private bool IsCellFree(List<Vector2Int> ocupiedCells, Vector2Int cell)
        {
            var result = ocupiedCells.FirstOrDefault(i => i.Equals(cell));

            return result != default(Vector2Int);
        }
    }
}