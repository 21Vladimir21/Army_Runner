using System;

namespace _Main._Scripts.UI
{
    public interface IView
    {
        void Close(Action callback = null);
        void Open(Action callback = null);
        void Init();
    }
}