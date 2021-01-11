using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace contextswitcher.loader
{
    /// <summary>
    /// Load the item shop
    /// </summary>
    public class ItemShopLoader : Loader
    {
        private bool _canBeLoad = false;
        private bool _isLoaded = false;


        private void Update()
        {
            /* Inputs handler */
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_canBeLoad)
                {
                    _contextSwitcher.ActivateContext(_contextNameToSwitch);

                    _isLoaded = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isLoaded)
                {
                    _contextSwitcher.DesactivateContext(_contextNameToSwitch);

                    _isLoaded = false;
                }
            }
        }

        public void OnTriggerEnter(Collider collider)
        {
            _canBeLoad = true;
        }

        public void OnTriggerExit(Collider collider)
        {
            _canBeLoad = false;
        }
    }
}

