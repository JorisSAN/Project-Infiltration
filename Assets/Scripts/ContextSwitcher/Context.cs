using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace contextswitcher
{
    /// <summary>
    /// A context is a part of the in game
    /// ex : in the spawn scene, there are multiples context (HUB / Loading screen / Skill tree ...)
    /// </summary>
    public class Context : MonoBehaviour
    {
        [SerializeField] private GameObject _context;
        [SerializeField] private string _name;
        private bool _isActive;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
        }

        public void ActivateContext()
        {
            _context.SetActive(true);
            _isActive = true;
        }

        public void DesactivateContext()
        {
            _context.SetActive(false);
            _isActive = false;
        }
    }
}
