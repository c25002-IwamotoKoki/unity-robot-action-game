using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class RandomAreaEnemyGenerator : MonoBehaviour
    {
        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _spawnRate;
        [SerializeField] private int _capacity;

        private float _spawnTimer;
        private int _lastSpawnIndex = -1;


        private void Awake()
        {
            TryGetComponent(out _enemyPool);
            _enemyPool.SetCapacity(_capacity);
        }

        private void Start()
        {
            _enemyPool.PrewarmPool();
        }

        private void Update()
        {
            _spawnTimer += Time.deltaTime;

            if(_spawnTimer >= _spawnRate)
            {
                _spawnTimer = 0;
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
            if (!_enemyPool.CanSpawn) return;

            EnemyBase enemy = _enemyPool.Spawn();

            int spawnIndex = GetSpawnIndex();
            Transform spawnPoint = _spawnPoints[spawnIndex];

            enemy.transform.SetPositionAndRotation(spawnPoint.position,spawnPoint.rotation);
        }

        private int GetSpawnIndex()
        {
            if(_spawnPoints.Length <= 1)
            {
                return 0;
            }

            //重複防止処理
            int index;
            do
            {
                index = Random.Range(0,_spawnPoints.Length);

            } while(index == _lastSpawnIndex);

            _lastSpawnIndex = index;

            return index;
        }

    }
}
