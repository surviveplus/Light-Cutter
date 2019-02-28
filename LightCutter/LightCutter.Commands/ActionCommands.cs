using System;
using System.Collections.Generic;
using System.Linq;
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

        public override string ToString()
        {
            return string.Join(" > ", (from a in this.Commands select a.Command));
        }

        public static ActionCommands FromCommands(string commands)
        {
            var r = new ActionCommands();
            foreach (var text in from c in commands.Split('>') select c.Trim())
            {
                // TODO: text and Parser - key value dictionary ?
                IActionCommand command = null;
                if (command == null) command = Cutting.CutCommand.FromCommand(text);
                if (command == null) command = Cutting.LastRangeCommand.FromCommand(text);
                if (command == null) command = Operations.WaitCommand.FromCommand(text);
                if (command == null) command = Sharing.CopyCommand.FromCommand(text);
                if (command == null) command = Sharing.SaveFileCommand.FromCommand(text);
                if (command == null) command = Targeting.TargetPrimaryMonitorCommand.FromCommand(text);
                if (command == null) command = Targeting.TargetScreenCommand.FromCommand(text);


                if (command != null)
                {
                    r.Commands.Add(command);
                }
                else
                {
                    throw new ArgumentException($"{text} is not command");
                } // end if
            }
            return r;
        }

    }
}
