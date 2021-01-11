using itemshop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace intemshop.ui.carousel
{
    [Serializable]
    public class ItemInfo
    {
        public string Uuid;
        public bool Unlocked;
        public int Cost;
        public Rarity Rarity;
        public string SpriteName;
        public UnityAction<ItemDisplayer> ActionWhenClicked;
        public ItemCollection Collection;
    }
}
