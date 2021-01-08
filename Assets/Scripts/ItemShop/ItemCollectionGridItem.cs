using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace itemshop
{
    public class ItemCollectionGridItem
    {
        private ItemCollection _collection;
        private int _x;
        private int _y;

        public ItemCollection Collection
        {
            get
            {
                return _collection;
            }
            set
            {
                _collection = value;
            }
        }

        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
    }
}