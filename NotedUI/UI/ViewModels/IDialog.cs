using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotedUI.UI.ViewModels
{
    public interface IDialog
    {
        event Action DialogClosed;
    }
}
