using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Net.Surviveplus.LightCutter.Commands
{
    public class ActionCommands
    {
        public List<IActionCommand> Commands { get; private set; } = new List<IActionCommand>();

        public void Do()
        {
            if (this.MustUac)
            {
                var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                var isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

                if (!isAdmin)
                {
                    //restart
                   var file = System.Reflection.Assembly.GetEntryAssembly().Location;
                    try
                    {
                        Application.Current.Shutdown();
                        Process.Start(new ProcessStartInfo(file) { UseShellExecute = true, Verb = "runas" });
                        return;
                    }
                    catch (Exception)
                    {
                        throw new MustRunAsAdminException();
                    }

                } // endif
            } // end if

            using (var state = new ActionState())
            {
                foreach (var command in this.Commands)
                {
                    command.Do(state);
                }
            }
        }

        public TextBlock DisplayCommand
        {
            get
            {
                var t = new TextBlock();

                var isFirst = true;
                foreach (var command in this.Commands)
                {
                    if (! isFirst)
                    {
                        t.Inlines.Add(" > ");
                    }
                    isFirst = false;

                    foreach (var item in command.DisplayCommand)
                    {
                        var uiElement = item as UIElement;
                        if (uiElement != null)
                        {
                            t.Inlines.Add(uiElement);
                        }
                        else
                        {
                            var text = item?.ToString();
                            t.Inlines.Add(text);
                        }
                    }

                }


                return t;

            }
        }


        public bool IsEnabled => !Commands.Any(c => !c.IsEnabled);

        public bool MustUac => Commands.Any(c => c.MustUac);

        public override string ToString()
        {
            return string.Join(" > ", (from a in this.Commands select a.Command));
        }

        public static ActionCommands FromCommands(string commands)
        {
            var r = new ActionCommands();
            foreach (var text in from c in commands.Split('>') select c.Trim())
            {
                IActionCommand command = null;
                if (command == null) command = Cutting.CutCommand.FromCommand(text);
                if (command == null) command = Cutting.LastRangeCommand.FromCommand(text);
                if (command == null) command = Editing.TrimColorCommand.FromCommand(text);
                if (command == null) command = Operations.WaitCommand.FromCommand(text);
                if (command == null) command = Sharing.CopyCommand.FromCommand(text);
                if (command == null) command = Sharing.SaveFileCommand.FromCommand(text);
                if (command == null) command = Sharing.OpenSavedFileCommand.FromCommand(text);
                if (command == null) command = Targeting.TargetConsoleCommand.FromCommand(text);
                if (command == null) command = Targeting.TargetPrimaryMonitorCommand.FromCommand(text);
                if (command == null) command = Targeting.TargetRemoteDesktopConnectionCommand.FromCommand(text);
                if (command == null) command = Targeting.TargetScreenCommand.FromCommand(text);
                if (command == null) command = Targeting.TargetVirtualMachineConnectionCommand.FromCommand(text);


                if (command != null)
                {
                    r.Commands.Add(command);
                }
                else
                {
                    throw new ArgumentException($"\"{text}\" is not command");
                } // end if
            }
            return r;
        }

    }
}
