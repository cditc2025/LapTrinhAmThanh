using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KienChi
{
    public class ObjectPool : MonoBehaviour
    {
        //* Số lượng pooledObject ban đầu
        private uint _initPoolSize = 1;
        public uint InitPoolSize => _initPoolSize;
        [SerializeField] protected Transform spawnPosition;

        //* PooledObject prefab
        [SerializeField] protected PooledObject objectToPool;
        public PooledObject ObjectToPool
        {
            get => objectToPool;
            set => objectToPool = value;
        }

        //* Stack lưu PooledObject
        protected Stack<PooledObject> stack = new Stack<PooledObject>();

        private void Start()
        {
            SetupPool();
        }

        //* Tạo PooledObject
        private void SetupPool()
        {
            if (objectToPool == null)
            {
                return;
            }

            PooledObject instance = null;

            for (int i = 0; i < _initPoolSize; i++)
            {
                instance = Instantiate(objectToPool, spawnPosition);
                instance.Pool = this;
                instance.gameObject.SetActive(false);
                stack.Push(instance);
            }
        }

        //* Trả về giá trị PooledObject
        public PooledObject GetPooledObject()
        {
            if (objectToPool == null)
            {
                return null;
            }

            //* Nếu stackpool không đủ, taọ PooledObjects để thêm vào stack 
            if (stack.Count == 0)
            {
                PooledObject newInstance = Instantiate(objectToPool, this.transform);
                newInstance.Pool = this;
                return newInstance;
            }
            //* Lấy ra giá trị cuối trong stack
            PooledObject nextInstance = stack.Pop();
            nextInstance.gameObject.SetActive(true);
            return nextInstance;
        }

        public void ReturnToPool(PooledObject pooledObject)
        {
            stack.Push(pooledObject);
            pooledObject.gameObject.SetActive(false);
        }
    }
}
