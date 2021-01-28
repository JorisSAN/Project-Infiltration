using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace itemshop.save
{
    public class SaveItem
    {
        public string _uuid;
        public bool _unlocked;
        public bool _consommable;
        public int _cost;
        public string _icon;
        public Rarity _rarity;
        public float _cooldown;
        public int _stock;
    }
}
