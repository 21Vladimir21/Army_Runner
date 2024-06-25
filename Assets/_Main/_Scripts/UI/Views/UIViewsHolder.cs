using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main._Scripts.UI
{
    public class UIViewsHolder : MonoBehaviour
    {
        [field: SerializeField] public List<AbstractView> Views { get; private set; }
    }
}