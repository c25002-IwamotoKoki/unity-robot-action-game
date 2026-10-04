using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class RandomAreaEnemyGenerator : MonoBehaviour
    {
        public event Action OnEnemyDied;

        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _spawnRate;
        [SerializeField] private int _capacity;

        private List<EnemyBase> _onFieldEnemy;
        private float _spawnTimer;
        private int _lastSpawnIndex = -1;


        private void Awake()
        {
            TryGetComponent(out _enemyPool);
            _enemyPool.SetCapacity(_capacity);
            _onFieldEnemy = new(_capacity);
        }

        private void Start()
        {
            _enemyPool.PrewarmPool();
        }

        private void Update()
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= _spawnRate)
            {
                _spawnTimer = 0;
                SpawnEnemy();
            }
        }

        private void OnDisable()
        {
            //敵が死ぬ前にScene移動をした場合などのメモリリーク防止のため
            //Listに保持している分をまとめて購読解除
            for (int i = 0; i < _onFieldEnemy.Count; i++)
            {
                if (_onFieldEnemy[i] != null)
                {
                    //ここではあくまでもメモリリーク防止のため死亡eventは発行しません
                    _onFieldEnemy[i].OnDied -= HandleOnDead;
                }
            }

            _onFieldEnemy.Clear();
        }

        public void SpawnEnemy()
        {
            if (!_enemyPool.CanSpawn) return;

            EnemyBase enemy = _enemyPool.Spawn();
            enemy.OnDied += HandleOnDead;

            _onFieldEnemy.Add(enemy);

            int spawnIndex = GetSpawnIndex();
            Transform spawnPoint = _spawnPoints[spawnIndex];

            enemy.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        }

        private int GetSpawnIndex()
        {
            if (_spawnPoints.Length <= 1)
            {
                return 0;
            }

            //重複防止処理
            int index;
            do
            {
                index = UnityEngine.Random.Range(0, _spawnPoints.Length);

            } while (index == _lastSpawnIndex);

            _lastSpawnIndex = index;

            return index;
        }

        private void HandleOnDead(EnemyBase enemy)
        {
            enemy.OnDied -= HandleOnDead;
            _onFieldEnemy.Remove(enemy);
            OnEnemyDied?.Invoke();
        }
    }
}
