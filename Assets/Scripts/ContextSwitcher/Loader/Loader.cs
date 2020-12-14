using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace contextswitcher.loader
{
    /// <summary>
    /// Load a context and/or a scene
    /// </summary>
    public class Loader : MonoBehaviour
    {
        [SerializeField] protected ContextSwitcher _contextSwitcher;
        [SerializeField] protected string _contextNameToSwitch;
    }
}
