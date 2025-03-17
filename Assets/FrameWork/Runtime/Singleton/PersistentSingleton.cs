using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FrameWork
{
    public class PersistentSingleton<T> : MonoBehaviour where T : PersistentSingleton<T>
    {
        protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        AutoLoadAsync();
                    }
                }
                return instance;
            }
        }

        #region �ڵ� �ε�
        private static async void AutoLoadAsync()
        {
            await AutoLoad();
        }

        private static async Task AutoLoad()
        {
            string key = typeof(T).Name;

            // ��巹���� Ű�� �����Ѵٸ�
            var locations = await Addressables.LoadResourceLocationsAsync(key).Task;

            if (locations.Count > 0)
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);

                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var obj = Instantiate(handle.Result);
                    instance = obj.GetComponent<T>();
                }
                else
                {
                    GameObject newInstance = new GameObject(typeof(T).Name);
                    instance = newInstance.AddComponent<T>();
                }
            }
            else
            {
                GameObject newInstance = new GameObject(typeof(T).Name);
                instance = newInstance.AddComponent<T>();
            }

            instance.Initialize();
            instance.SetupDontDestroy();
        }
        #endregion

        protected virtual void Awake()
        {
            if (instance == null || instance == this)
            {
                instance = this as T;
                instance.Initialize();
                SetupDontDestroy();
            }
            else
            {
                if (this != instance)
                {
#if UNITY_EDITOR
                    Debug.Log($"{typeof(T).Name} : �̹� �̱����� �����Ͽ� �����˴ϴ�.");
#endif
                    Destroy(gameObject);
                }
            }
        }

        private void SetupDontDestroy()
        {
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                transform.SetParent(null);
#if UNITY_EDITOR
                Debug.LogError($"{typeof(T).Name} : �ı����� �ʴ� ������Ʈ�� �θ� ������ �� ��ȯ �� ������ �� �ֽ��ϴ�.");
#endif
            }
        }

        protected virtual void Initialize()
        {

        }
    }
}