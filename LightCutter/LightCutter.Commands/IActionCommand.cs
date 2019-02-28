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

        // TODO: System.Windows.Controls.TextBlock DisplayCommand { get; }

    } // end interface

} // end namespace
