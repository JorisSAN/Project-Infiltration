using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace intemshop.ui.carousel
{
    public class ItemShopCarouselFiller : MonoBehaviour
    {
        [SerializeField] private ItemShopCarousel _carousel = default;
        [SerializeField] private ItemInfo[] _infos = default;
        public void FillCarouselWithViews(ItemInfo[] infos)
        {
            _infos = infos;
            _carousel.SetElementsToDisplay(_infos);
            _carousel.SetupCarousel();
        }
    }
}