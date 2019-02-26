using ActionsSample.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsSample
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

        public override string ToString()
        {
            return string.Join(" > ",  (from a in this.Commands select a.Command) );
        }

        public static ActionCommands FromCommands(string commands)
        {
            var r = new ActionCommands();
            foreach (var text in from c in commands.Split('>') select c.Trim() )
            {
                // TODO: text and Parser - key value dictionary ?
                IActionCommand command = null;
                if(command == null ) command = CutCommand.FromCommand(text);
                if(command == null ) command = SaveFileCommand.FromCommand(text);
                if(command == null ) command = TargetPrimaryMonitorCommand.FromCommand(text);
                if(command == null ) command = TargetScreenCommand.FromCommand(text);
                if(command == null ) command = WaitCommand.FromCommand(text);


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
