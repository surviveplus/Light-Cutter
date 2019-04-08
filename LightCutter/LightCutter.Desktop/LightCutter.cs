using Net.Surviveplus.LightCutter.Desktop.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public static void SaveCommands()
        {
            var actions = Newtonsoft.Json.JsonConvert.SerializeObject((from a in LightCutter.Commands select a.Commands).ToArray());
            Settings.Default.Actions = actions;
        }

        public static void LoadCommands()
        {
            if (string.IsNullOrWhiteSpace(Settings.Default.Actions)) return;

            string[] actions = null;
            try
            {
                actions = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(Settings.Default.Actions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadCommands ERROR");
                Debug.WriteLine(ex.ToString());
            }

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    try
                    {
                        AddAction(action);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("LoadCommands AddAction ERROR");
                        Debug.WriteLine(ex.ToString());
                    }
                }
            } // end if

        }

        public static void InitializeActionCommands()
        {
            LightCutter.Commands = new List<Models.ActionModel>();
            LightCutter.ActionCommands = new Dictionary<string, Commands.ActionCommands>();

            LoadCommands();

            if(LightCutter.Commands.Count == 0)
            {
                ResetActions();
            }
        }

        public static void ResetActions()
        {
            Settings.Default.Actions = string.Empty;
            LightCutter.Commands = new List<Models.ActionModel>();
            LightCutter.ActionCommands = new Dictionary<string, Commands.ActionCommands>();

            AddAction("Screen > Cut > Copy");
            AddAction("Screen > Cut > Save");
            AddAction("Screen > Last Range > Save");

            //AddAction("Wait > Screen > Cut > Copy");
            AddAction("Wait > Screen > Cut > Save");
            AddAction("Wait > Screen > Last Range  > Save");

            //AddAction("Primary Monitor > Save");
            //AddAction("Wait > Primary Monitor > Save");
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

            SaveCommands();
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

            SaveCommands();
        }

        public static void ReplaceOrAddAction(Models.ActionModel original, string newCommands)
        {
            if (original != null &&
                LightCutter.Commands.Contains(original))
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

            SaveCommands();
        }

        public static ObservableCollection<Models.NotificationModel> Notifications = new ObservableCollection<Models.NotificationModel>();

        public static bool TryDo( Action action, Func<string> getFailedActionDisplayName )
        {
            try
            {
                action.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                LightCutter.Notifications.Insert(0, Models.NotificationModel.FromException(ex, getFailedActionDisplayName.Invoke() ));
                LightCutter.AddNotifications?.Invoke(null, EventArgs.Empty);
                return false;
            }
        }

        public static event EventHandler<EventArgs> AddNotifications;
        
    }
}
