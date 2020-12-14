using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace contextswitcher
{
    /// <summary>
    /// Switch between contexts in a scene
    /// </summary>
    public class ContextSwitcher : MonoBehaviour
    {
        [SerializeField] private Context _mainContext = default; // main context is always active (often w/ the player in)
        [SerializeField] private List<Context> _otherContexts = new List<Context>();

        public void Awake()
        {
            DesactivateAllContext();
            ActivateMainContext();
        }

        public void ActivateMainContext()
        {
            _mainContext.ActivateContext();
        }

        public void DesactivateAllContext()
        {
            foreach (Context c in _otherContexts)
            {
                c.DesactivateContext();
            }
        }

        /// <summary>
        /// Activate context at given index
        /// &
        /// Desactivate others contexts
        /// </summary>
        /// <param name="contextIndex"></param>
        public void ActivateContext(int contextIndex)
        {
            if (_otherContexts.Count > 0 && _otherContexts.Count < contextIndex)
            {
                for (int i = 0; i < _otherContexts.Count; i++)
                {
                    if (i == contextIndex)
                    {
                        _otherContexts[contextIndex].ActivateContext();
                    }
                    else
                    {
                        _otherContexts[i].DesactivateContext();
                    }
                }

            }
        }

        /// <summary>
        /// Desactivate context at given index
        /// </summary>
        /// <param name="contextIndex"></param>
        public void DesactivateContext(int contextIndex)
        {
            if (_otherContexts.Count > 0 && _otherContexts.Count < contextIndex)
            {
                if (_otherContexts[contextIndex].IsActive)
                {
                    _otherContexts[contextIndex].DesactivateContext();
                    ActivateMainContext();
                }
            }
        }

        /// <summary>
        /// Activate context with given name
        /// &
        /// Desactivate others contexts
        /// </summary>
        /// <param name="contextName"></param>
        public void ActivateContext(string contextName)
        {
            if (ContainsContextWithName(contextName))
            {
                foreach (Context c in _otherContexts)
                {
                    if (c.Name.Equals(contextName))
                    {
                        c.ActivateContext();
                    }
                    else
                    {
                        c.DesactivateContext();
                    }
                }

            }
        }

        /// <summary>
        /// Desactivate context with given name
        /// </summary>
        /// <param name="contextName"></param>
        public void DesactivateContext(string contextName)
        {
            Context context = GetContextWithName(contextName);
            if (!(context is null))
            {
                context.DesactivateContext();
                ActivateMainContext();
            }
        }

        public bool ContainsContextWithName(string contextName)
        {
            foreach (Context c in _otherContexts)
            {
                if (c.Name.Equals(contextName))
                {
                    return true;
                }
            }
            return false;
        }

        public Context GetContextWithName(string contextName)
        {
            if (!ContainsContextWithName(contextName)) return null;

            foreach (Context c in _otherContexts)
            {
                if (c.Name.Equals(contextName))
                {
                    return c;
                }
            }

            return null;
        }
    }
}
