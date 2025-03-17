using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public static class TextureExtensions
    {
        /// <summary>
        /// Texture2D�� ũ�⸦ ������¡
        /// </summary>
        public static Texture2D ResizeTexture(this Texture2D original, int width, int height)
        {
            // RenderTexture�� ����Ͽ� ������¡�� ó��
            RenderTexture rt = new RenderTexture(width, height, 24);
            Graphics.Blit(original, rt); 

            // RenderTexture�� �ؽ�ó2D�� ��ȯ
            Texture2D newTexture = new Texture2D(width, height);
            RenderTexture.active = rt;
            newTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            newTexture.Apply();

            // RenderTexture�� ��ó��
            RenderTexture.active = null;
            rt.Release();

            return newTexture;
        }
    }

}
