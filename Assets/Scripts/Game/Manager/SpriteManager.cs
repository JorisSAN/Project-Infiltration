using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.manager
{
    public class SpriteManager : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites = new List<Sprite>(0);

        public static SpriteManager Instance { get; private set; } // Singleton for the moment : to change

        private void Awake()
        {
            Instance = this;
        }

        public Sprite GetSprite(string spriteName)
        {
            foreach (Sprite sprite in _sprites)
            {
                if (sprite.name.Equals(spriteName))
                {
                    Debug.Log("Found sprite : " + spriteName);
                    return sprite;
                }
            }

            return null;
        }
    }
}
