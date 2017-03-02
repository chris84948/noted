using System;

namespace NotedUI.UI.ViewModels
{
    public interface IDialog
    {
        event Action<IDialog> DialogClosed;
    }
}
