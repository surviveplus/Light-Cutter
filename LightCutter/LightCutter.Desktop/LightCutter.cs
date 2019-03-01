using Net.Surviveplus.LightCutter.Desktop.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Desktop
{
    public class LightCutter
    {

        public static List<Models.ActionModel> Commands;
        public static Dictionary<string, Commands.ActionCommands> ActionCommands ;

        public static void InitializeActionCommands()
        {
            LightCutter.Commands = new List<Models.ActionModel>();
            LightCutter.ActionCommands = new Dictionary<string, Commands.ActionCommands>();

            AddAction("Screen > Cut > Copy");
            AddAction("Screen > Cut > Save");
            AddAction("Screen > Last Range > Save");

            //AddAction("Wait > Screen > Cut > Copy");
            AddAction("Wait > Screen > Cut > Save");
            AddAction("Wait > Screen > Last Range  > Save");

            AddAction("Primary Monitor > Save");
            AddAction("Wait > Primary Monitor > Save");
        }

        private static long lastId = 0;

        public static void AddAction(string commands)
        {
            LightCutter.Commands.Add(new Models.ActionModel { Id = LightCutter.lastId, Commands = commands });
            LightCutter.lastId += 1;

            if (!LightCutter.ActionCommands.ContainsKey(commands))
            {
                var action = Surviveplus.LightCutter.Commands.ActionCommands.FromCommands(commands);
                LightCutter.ActionCommands[commands] = action;
            }
        }

        public static void RemoveAction(Models.ActionModel action)
        {
            if (LightCutter.Commands.Contains(action))
            {
                LightCutter.Commands.Remove(action);
            }

            if (!LightCutter.Commands.Contains(action))
            {
                LightCutter.ActionCommands.Remove(action.Commands);
            }
        }

        public static void ReplaceOrAddAction(Models.ActionModel original, string newCommands)
        {
            if (LightCutter.Commands.Contains(original))
            {
                var index = LightCutter.Commands.IndexOf(original);
                LightCutter.Commands.Remove(original);

                if (!(from a in LightCutter.Commands
                      where a.Commands == original.Commands
                      select a).Any())
                {
                    if(original.Commands != newCommands)
                    {
                        LightCutter.ActionCommands.Remove(original.Commands);
                    }
                }
                LightCutter.Commands.Insert(index, new Models.ActionModel { Id = LightCutter.lastId, Commands = newCommands });
            }
            else
            {
                LightCutter.Commands.Add(new Models.ActionModel { Id = LightCutter.lastId, Commands = newCommands });
            }
            LightCutter.lastId += 1;

            if (!LightCutter.ActionCommands.ContainsKey(newCommands))
            {
                var action = Surviveplus.LightCutter.Commands.ActionCommands.FromCommands(newCommands);
                LightCutter.ActionCommands[newCommands] = action;
            }

        }


    }
}
