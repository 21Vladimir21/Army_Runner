using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main._Scripts.UI
{
    [Serializable]
    public class UIViews
    {
        [field: Header("Views")]
        [field: SerializeField]
        public List<AbstractView> Views { get; private set; }
    }
}