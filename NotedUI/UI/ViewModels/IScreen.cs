using NotedUI.Controls;
using System;

namespace NotedUI.UI.ViewModels
{
    public interface IScreen
    {
        event Action<IScreen, eTransitionType> ChangeScreen;
    }
}
