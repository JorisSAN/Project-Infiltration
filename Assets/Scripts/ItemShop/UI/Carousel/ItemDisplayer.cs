using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using carousel;
using intemshop.ui.carousel;
using UnityEngine.UI;
using itemshop;
using game.manager;
using itempshop.ui;
using UnityEngine.Events;

namespace intemshop.ui.carousel
{
    public class ItemDisplayer : ASlotListElementDisplayer<ItemInfo>
    {
        private ItemInfo _infos;
        private NodeStatus _status;
        private ItemCollection _collection;

        [SerializeField] private Image _backgroundRarity = default;
        [SerializeField] private Text _title = default;
        [SerializeField] private Image _icon = default;
        [SerializeField] private Text _costAmount = default;
        [SerializeField] private Button _purchaseButton = default;

        [Header("Rarity Colors")]
        [SerializeField] private Color _commonColor = default;
        [SerializeField] private Color _rareColor = default;
        [SerializeField] private Color _epicColor = default;
        [SerializeField] private Color _legendaryColor = default;

        public ItemCollection Collection
        {
            get
            {
                return _collection;
            }
        }

        public Color CommonColor
        {
            get
            {
                if (_commonColor == null) _commonColor = Color.cyan;
                return _commonColor;
            }
        }

        public Color RareColor
        {
            get
            {
                if (_rareColor == null) _rareColor = Color.green;
                return _rareColor;
            }
        }

        public Color EpicColor
        {
            get
            {
                if (_epicColor == null) _epicColor = Color.magenta;
                return _epicColor;
            }
        }

        public Color LegendaryColor
        {
            get
            {
                if (_legendaryColor == null) _legendaryColor = Color.yellow;
                return _legendaryColor;
            }
        }

        // METHODS

        public override void Replenish(ItemInfo elementToInsert)
        {
            if (elementToInsert == null)
            {
                gameObject.SetActive(false);
                return;
            }
            _infos = elementToInsert;
            gameObject.SetActive(true);

            SetBackground(elementToInsert.Rarity);
            _title.text = elementToInsert.Uuid;
            _icon.sprite = GetSprite(elementToInsert.SpriteName);
            _costAmount.text = elementToInsert.Cost.ToString();
            SetActionWhenClicked(elementToInsert.ActionWhenClicked);
            _collection = elementToInsert.Collection;
            SetStatus();
        }

        public void SetBackground(Rarity rarity)
        {
            _backgroundRarity.color = GetColor(rarity);
        }

        public Color GetColor(Rarity rarity)
        {
            Color color = CommonColor;
            switch (rarity)
            {
                case Rarity.COMMON:
                    color = CommonColor;
                    break;
                case Rarity.RARE:
                    color = RareColor;
                    break;
                case Rarity.EPIC:
                    color = EpicColor;
                    break;
                case Rarity.LEGENDARY:
                    color = LegendaryColor;
                    break;
            }

            return color;
        }

        public Sprite GetSprite(string spriteName)
        {
            Sprite sprite = SpriteManager.Instance.GetSprite(spriteName);
            return sprite;
        }

        public void SetActionWhenClicked(UnityAction<ItemDisplayer> action)
        {
            _purchaseButton.onClick.RemoveAllListeners();
            _purchaseButton.onClick.AddListener(() =>
            {
                action(this);
            });
        }

        public void SetStatus()
        {
            SetStatus(NodeStatus.Locked);

            if (Collection.Item.Unlocked)
            {
                SetStatus(NodeStatus.Unlocked); // Fully purchased
            }
            else if (Collection.Item.IsAvailable())
            {
                SetStatus(NodeStatus.Purchasable); // Avaialable for purchase
            }
        }

        public void SetStatus(NodeStatus status)
        {
            this._status = status;
        }

        public NodeStatus GetStatus()
        {
            return _status;
        }
    }
}
