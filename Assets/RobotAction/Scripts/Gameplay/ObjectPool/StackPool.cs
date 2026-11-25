using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobotAction.Gameplay.ObjectPool
{
    /// <summary>
    /// このPoolを生成した時点で自動的にCapacityの分Prewarm処理が入ります。
    /// <br/>また、Poolアイテムの親の名前はPrefab名 + InPoolParentとなるため多少のGCが発生します
    /// <br/>上記に加え、Pool内のアイテムが空ならnullを返すため注意
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StackPool<T> where T : Component
    {
        public bool CanRent => _pool.Count > 0;

        private readonly Stack<T> _pool;
        private readonly HashSet<T> _poolItemSet;
        private readonly T _prefab;

        /// <summary>
        /// <paramref name="onCreatedItem"/>はDI用関数を入れることを想定しています。
        /// オブジェクト生成時、SetActive(false)前に呼ばれます。
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="capacity"></param>
        /// <param name="onCreatedItem"></param>
        public StackPool(T prefab, int capacity,Action<T,StackPool<T>> onCreatedItem = null)
        {
            _pool = new Stack<T>(capacity);
            _poolItemSet = new HashSet<T>(capacity);
            _prefab = prefab;

            Prewarm(capacity,onCreatedItem);
        }

        private void Prewarm(int capacity,Action<T,StackPool<T>> onCreatedItem = null)
        {
            GameObject inPoolItemsParent = new GameObject($"{_prefab.name}InPoolParent");

            for(int i = 0; i < capacity; i ++)
            {
                T item = UnityEngine.Object.Instantiate(_prefab, inPoolItemsParent.transform);
                onCreatedItem?.Invoke(item,this);
                item.gameObject.SetActive(false);

                _pool.Push(item);
                _poolItemSet.Add(item);
            }
        }

        /// <summary>
        /// pool内に何もないならnullを返します
        /// <br/>nullチェックはこれを使う側で行うことを想定しています
        /// </summary>
        /// <returns></returns>
        public T Rent()
        {
            if (_pool.Count <= 0) return null;

            T item = _pool.Pop();
            _poolItemSet.Remove(item);
            item.gameObject.SetActive(true);

            return item;
        }

        public void Release(T item)
        {
            if (item == null) return;

            if (!_poolItemSet.Add(item)) return;

            item.gameObject.SetActive(false);
            _pool.Push(item);
        }
    }
}
