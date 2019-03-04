using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands
{
    public interface IActionCommand
    {
        void Do(ActionState state);

        string Command { get; }

        IEnumerable<object> DisplayCommand { get; }

        bool IsEnabled { get; }

        bool MustUac { get; } 

    } // end interface

} // end namespace
