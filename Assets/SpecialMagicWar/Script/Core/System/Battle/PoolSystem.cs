using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialMagicWar.Core
{
    public class PoolSystem : MonoBehaviour, ICoreSystem
    {
        private Dictionary<string, Stack<GameObject>> _objectPool = new Dictionary<string, Stack<GameObject>>();

        public void Initialize()
        {

        }

        public void Deinitialize()
        {
            // ������Ʈ Ǯ ����ֱ�
            foreach (var stack in _objectPool.Values)
            {
                foreach (var obj in stack)
                {
                    Destroy(obj);
                }
                stack.Clear();
            }
            _objectPool.Clear();
        }

        public GameObject Spawn(GameObject obj, Transform parent = null)
        {
            string key = obj.name;

            if (_objectPool.ContainsKey(key) && _objectPool[key].Count > 0)
            {
                GameObject poolObj = _objectPool[key].Pop();
                
                if (parent != null && poolObj.transform.parent != parent)
                    poolObj.transform.parent = parent;

                poolObj.SetActive(true);
                return poolObj;
            }
            else
            {
                if (parent == null) parent = transform;

                GameObject newObj = Instantiate(obj, parent);
                newObj.name = key;

                if (!_objectPool.ContainsKey(key))
                {
                    _objectPool[key] = new Stack<GameObject>();
                }

                return newObj;
            }
        }

        #region Spawn �޼��� ���ӽð� ����
        public GameObject Spawn(GameObject obj, float duration, Transform parent = null)
        {
            var newObj = Spawn(obj, parent);

            if (duration > 0)
            {
                StartCoroutine(CoSpawn(newObj, duration));
            }

            return newObj;
        }

        private IEnumerator CoSpawn(GameObject obj, float duration = 0)
        {
            yield return new WaitForSeconds(duration);
            DeSpawn(obj);
        }
        #endregion

        public void DeSpawn(GameObject obj)
        {
            string key = obj.name;

            if (!_objectPool.ContainsKey(key))
            {
                _objectPool[key] = new Stack<GameObject>();
            }

            _objectPool[key].Push(obj);
            obj.SetActive(false);
        }
    }
}