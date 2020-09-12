using UnityEngine;

namespace Bsr.Unity.Extensions.Unity
{
    public static class SpriteHelper
    {
        public static Sprite WhiteSquare(float size)
        {
            size = Mathf.Clamp01(size);
            return Sprite.Create(Texture2D.whiteTexture, new Rect(Vector2.zero, new Vector2(size,size)), new Vector2(0.5f, 0.5f));
        }
        
        public static Sprite GraySquare(float size)
        {
            size = Mathf.Clamp01(size);
            return Sprite.Create(Texture2D.grayTexture, new Rect(Vector2.zero, new Vector2(size,size)), new Vector2(0.5f, 0.5f));
        }
        
        public static Sprite BlackSquare(float size)
        {
            size = Mathf.Clamp01(size);
            return Sprite.Create(Texture2D.blackTexture, new Rect(Vector2.zero, new Vector2(size,size)), new Vector2(0.5f, 0.5f));
        }
        
        public static Sprite RedSquare(float size)
        {
            size = Mathf.Clamp01(size);
            return Sprite.Create(Texture2D.redTexture, new Rect(Vector2.zero, new Vector2(size,size)), new Vector2(0.5f, 0.5f));
        }
    }
}