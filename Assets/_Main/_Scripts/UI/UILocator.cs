using System;
using System.Collections.Generic;
using _Main._Scripts.Services;
using UnityEngine;

namespace _Main._Scripts.UI
{
    public class UILocator : IService
    {
        private readonly Dictionary<Type, IView> _uiViews = new();

        private IView _currentView;
        private IView _currentPopupView;

        private IView _lastClosedUI;

        public UILocator(UIViews uiViews)
        {
            foreach (var view in uiViews.Views)
                _uiViews.Add(view.GetType(), view);
        }

        public void OpenUI(Type type, Action openedCallback = null, Action closedCallback = null)
        {
            _lastClosedUI = _currentView;
            _currentView?.Close(closedCallback);

            if (TryGetView(type, out var view))
            {
                _currentView = view;
                view.Open(openedCallback);
            }
        }

        private bool TryGetView(Type type, out AbstractView abstractView)
        {
            if (_uiViews.TryGetValue(type, out var view))
            {
                abstractView = (AbstractView)view;
                return true;
            }

            abstractView = null;
            return false;
        }

        public AbstractView GetViewByType(Type type)
        {
            return TryGetView(type, out var spawnedView) ? spawnedView : null;
        }

        public void OpenLastUI(Action openedCallback = null, Action closedCallback = null)
        {
            _currentView?.Close(closedCallback);

            _currentView = _lastClosedUI;
            _lastClosedUI.Open(openedCallback);
        }
    }
}