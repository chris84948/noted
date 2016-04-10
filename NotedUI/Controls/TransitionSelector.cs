using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NotedUI.Controls
{
    public enum eTransitionType
    {
        SlideInFromLeft,
        SlideOutFromRight
    }

    public class TransitionSelector
    {
        private static Dictionary<eTransitionType, Transition> _transitionTypes =
            new Dictionary<eTransitionType, Transition>()
            {
                { eTransitionType.SlideInFromLeft, new SlideTransition() { Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                                                                           StartPoint = new Point(-1, 0) } },
                { eTransitionType.SlideOutFromRight, new SlideTransition() { Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                                                                             EndPoint = new Point(-1, 0),
                                                                             IsNewContentTopmost = false } },
            };

        public static Transition Get(eTransitionType transition)
        {
            if (_transitionTypes.ContainsKey(transition))
                return _transitionTypes[transition];
            else
                return null;
        }
    }
}
