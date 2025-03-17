using FrameWork;
using FrameWork.VisualNovel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SpecialMagicWar.Tutorial
{
    /// <summary>
    /// Tutorial ���� �����ϴ� �Ŵ���
    /// </summary>
    public class TutorialSceneManager : MonoBehaviour
    {
        [SerializeField] private AssetReferenceT<ChapterTemplate> _chapterTemplate;

        private void Start()
        {
            AddressableAssetManager.Instance.GetScriptableObject<ChapterTemplate>(_chapterTemplate.RuntimeKey.ToString(), (template) =>
            {
                VisualNovelManager.Instance.Load(template);
            });
        }
    }
}