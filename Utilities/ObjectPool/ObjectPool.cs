using System.Collections.Generic;
using UnityEngine;
using Utilities.Singleton;

namespace Utilities.ObjectPool
{
    /// <summary>
    /// 此类实现了对象池模式，用于创建和管理大量的游戏对象
    /// </summary>
    public class ObjectPool : Singleton<ObjectPool>
    {
        private readonly GameObject _prefab; // 预制体对象，用来创建新的游戏对象
        private readonly int _initialPoolSize; // 初始对象池大小，即预先生成的游戏对象数量
        private readonly bool _canGrow; // 是否可以动态扩展对象池大小
        private readonly Queue<GameObject> _pool = new(); // 游戏对象池

        // 私有无参构造函数，避免从外部创建对象池实例
        private ObjectPool()
        {
            _prefab = null;
            _initialPoolSize = 0;
            _canGrow = false;
        }

        // 构造函数，创建对象池实例，并初始化预制体对象、对象池大小、是否可扩展等参数
        public ObjectPool(GameObject prefab, int initialPoolSize, bool canGrow)
        {
            _prefab = prefab;
            _initialPoolSize = initialPoolSize;
            _canGrow = canGrow;

            InitializePool(); // 初始化对象池
        }

        // 从对象池中获取未被使用的游戏对象，如果对象池已满并且可以扩展，将会创建新的游戏对象
        public GameObject GetObject()
        {
            lock (_pool)
            {
                if (_pool.Count <= 0) return _canGrow ? AddObjectToPool() : null;
                var obj = _pool.Dequeue();
                obj.SetActive(true);
                return obj;

            }
        }

        // 将游戏对象返回到对象池中
        public void ReturnObject(GameObject obj)
        {
            lock (_pool)
            {
                obj.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        // 初始化对象池，创建一定数量的游戏对象并加入到游戏对象池中
        private void InitializePool()
        {
            for (var i = 0; i < _initialPoolSize; i++)
            {
                AddObjectToPool();
            }
        }

        // 向对象池中添加新的游戏对象
        private GameObject AddObjectToPool()
        {
            var obj = GameObject.Instantiate(_prefab);
            obj.SetActive(false);
            lock (_pool)
            {
                _pool.Enqueue(obj);
            }
            return obj;
        }
    }
}
