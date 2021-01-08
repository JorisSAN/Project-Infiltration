using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace itemshop
{
    public class ItemCollectionGrid
    {
        /// <summary>
        /// The grid organized as grid[y, x].
        /// </summary>
        private ItemCollection[,] _grid;

        public ItemCollectionGrid(ItemCollection[,] grid)
        {
            _grid = grid;
        }

        public ItemCollection[,] Grid
        {
            get
            {
                return _grid;
            }
        }

        public int Width
        {
            get { return _grid.GetLength(0); }
        }
        public int Height
        {
            get { return _grid.GetLength(1); }
        }

        // METHODS

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <returns>The collection.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public ItemCollection GetCollection(int x, int y)
        {
            return _grid[x, y];
        }

        /// <summary>
        /// Gets all collections available.
        /// </summary>
        /// <returns>All items stored in the grid.</returns>
        public List<ItemCollectionGridItem> GetAllItems()
        {
            List<ItemCollectionGridItem> items = new List<ItemCollectionGridItem>();

            for (int x = 0; x < _grid.GetLength(0); x++)
            {
                for (int y = 0; y < _grid.GetLength(1); y++)
                {
                    if (_grid[x, y] != null) items.Add(new ItemCollectionGridItem
                    {
                        X = x,
                        Y = y,
                        Collection = _grid[x, y]
                    });
                }
            }

            return items;
        }
    }
}
