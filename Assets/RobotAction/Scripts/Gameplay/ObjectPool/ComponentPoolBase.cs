using RobotAction.Gameplay.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace RobotAction.Gameplay.ObjectPool
{
    public abstract class ComponentPoolBase<T> : MonoBehaviour where T : Component,IPoolable<T>
    {
        [SerializeField] protected T _poolablePrefab;

        protected GameObject _inPoolObjectsParent;
        private static readonly Vector3 _poolObjectsParentPos = new (0, -100, 0);

        protected int _defaultCapacity;
        protected int _maxCapacity;

        private IObjectPool<T> _pool;

        public IObjectPool<T> Pool => _pool ??= CreatePool();

        private IObjectPool<T> CreatePool()
        {
            return new ObjectPool<T>(

                createFunc: OnCreatePool,
                actionOnGet: OnGetInPool,
                actionOnRelease: OnReturnToPool,
                actionOnDestroy: OnMaxCapacityOver,
                collectionCheck: true,
                defaultCapacity: _defaultCapacity,
                maxSize: _maxCapacity
            );
        }

        protected virtual T OnCreatePool()
        {
            if (GetCountAll() >= _maxCapacity)
            {
                return null;
            }

            T instance = Instantiate(_poolablePrefab);

            instance.transform.SetParent(_inPoolObjectsParent.transform, true);
            instance.OnCreated(_pool);
           
            return instance;
        }

        protected virtual void OnGetInPool(T instance)
        {
            if (!instance)
            {
                return;
            }

            instance.gameObject.SetActive(true);
            instance.OnGet();
        }

        protected virtual void OnReturnToPool(T instance)
        {
            instance.OnReturn();
            instance.gameObject.SetActive(false);
        }

        protected virtual void OnMaxCapacityOver(T instance)
        {
            Destroy(instance);
        }

        protected void SetDefaultCapacity(int defaultCapacity)
        {
            _defaultCapacity = defaultCapacity;
        }

        protected void SetMaxCapacity(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
        }

        protected virtual void InitializePoolFlow()
        {
            //必要な分Instantiateして生成した分を非アクティブにして待機させておく処理

            _inPoolObjectsParent = new GameObject("InPoolObjectsParents");
            //オブジェクトプール内のオブジェクトの親を他のオブジェクトと干渉しない遠い位置に置く
            _inPoolObjectsParent.transform.position = _poolObjectsParentPos;

            List<T> tempTList = ListPool<T>.Get();

            for (int i = 0; i < _defaultCapacity; i++)
            {
                tempTList.Add(Pool.Get());
            }

            for (int i = 0; i < tempTList.Count; i++)
            {
                _pool.Release(tempTList[i]);

            }

            ListPool<T>.Release(tempTList);
        }

        /// <summary>
        /// ObjectPoolに存在する物の総数を返す関数です。
        /// <br/>Avtiveと非Avtiveの総数を返します。
        /// </summary>
        /// <returns></returns>
        protected virtual int GetCountAll()
        {
            if (_pool is ObjectPool<T> pool)
            {
                return pool.CountAll;
            }

            return -1;
        }

    }//EndClass
}
