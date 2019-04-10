using Net.Surviveplus.LightCutter.Desktop.Properties;
using Net.Surviveplus.LightCutter.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Net.Surviveplus.LightCutter.Desktop.Pages
{
    /// <summary>
    /// ActionPage.xaml の相互作用ロジック
    /// </summary>
    public partial class ActionPage : Page
    {
        public ActionPage( MainWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }
        private MainWindow parentWindow;

        public bool MustRefreshActions { get; set; } = false;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var b = App.Current.MainWindow as BackgroundWindow;

            this.shortcutA.Visibility = b.ShortcutOpenActionPanel.Enabled.ToVisibleOrCollapsed();
            this.shortcutA.DataContext = b.ShortcutOpenActionPanelViewModel;
            this.shortcutAError.Visibility =(! b.ShortcutOpenActionPanel.Enabled ).ToVisibleOrCollapsed();

            var defaultShortcut = new DefaultShortcutViewModel
            {
                HotkeyVisibility = b.ShortcutStartDefaultAction.Enabled.ToVisibleOrCollapsed(),
                Hotkey = b.ShortcutStartDefaultActionViewModel,
                ErrorVisibility = (!b.ShortcutStartDefaultAction.Enabled).ToVisibleOrCollapsed()
            };

            if (this.MustRefreshActions || 
                this.Actions.Children.Count == 0)
            {
                this.Actions.Children.Clear();
                int index = 0;
                foreach (var m in LightCutter.Commands)
                {
                    index += 1; 
                    var id = m.Id;
                    var command = m.Commands;
                    var action = LightCutter.ActionCommands[command];

                    var actionButton = new UI.Parts.ActionButton { Visibility = Visibility.Visible } ;
                    actionButton.Tag = action;
                    actionButton.Content = action.DisplayCommand;
                    actionButton.ButtonIsEnabled = action.IsEnabled;
                    actionButton.MustUac = action.MustUac;

                    // 1~9, 0,A~Z ?
                    var accessText = string.Empty;
                    if( index < 10)
                    {
                        accessText = $"_{index}.";
                    }
                    else if (index == 10)
                    {
                        accessText = $"_0.";
                    }
                    else
                    {
                        var number = (index - 11) % 26;
                        accessText = $"_{ Convert.ToChar(number + 0x41) }.";
                    }

                    var model = new ActionViewModel { AccessText = accessText, Name = command, DefaultShortcut = defaultShortcut, DefaultShortcutVisibility = Settings.Default.DefaultActionName == command ? Visibility.Visible : Visibility.Collapsed};
                    actionButton.DataContext = model;
                    actionButton.Click += (sender2, e2) => {
                        using (new WindowHide(this.parentWindow))
                        {
                            LightCutter.TryDo(() => action.Do(), () => action.ToString());
                        } // end using
                    };
                    actionButton.IsDefaultChanged += (sender2, e2) => {
                        if (model.DefaultShortcutVisibility == Visibility.Visible)
                        {
                            Settings.Default.DefaultActionName = command;
                            Settings.Default.SaveAndUpdateCommandsSettings();
                        }
                    };
                    actionButton.EditButtonClick += (sender2, e2) => {
                        this.parentWindow.mainFrame.Content = new EditActionPage(this.parentWindow, this, model.AccessText, m);
                    
                    };
                    this.Actions.Children.Add(actionButton);
                } // next kvp
            }

            this.RefreshEditMode();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            foreach (var child in this.Actions.Children)
            {
                var actionButton = child as UI.Parts.ActionButton;
                if (actionButton != null)
                {
                    var action = actionButton.Tag as Commands.ActionCommands;
                    actionButton.ButtonIsEnabled = action.IsEnabled;
                }
            }

        }


        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.EditButton.IsChecked = false;
            this.parentWindow.ShowSetting();
        }

        private void RefreshEditMode()
        {
            var button = this.EditButton;
            var edit = button.IsChecked.Value;

            foreach (var child in this.Actions.Children)
            {
                var actionButton = child as UI.Parts.ActionButton;
                if(actionButton != null)
                {
                    actionButton.ShowDefaultActionSelection = edit;
                }
            }

            this.actionOperationArea.Height = edit ? new GridLength( 50) :  new GridLength( 0 );
        }

        private void EditButton_Checked(object sender, RoutedEventArgs e)
        {
            this.RefreshEditMode();
        }

        private void EditButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.RefreshEditMode();
        }


        private void HelpLink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/surviveplus/Light-Cutter/blob/master/HowToUse/HowToUse.md");
        }

        private void AddActionButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.mainFrame.Content = new EditActionPage(this.parentWindow, this, string.Empty, null);
        }

        private void RecetActionsButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.ResetActions();
            this.parentWindow.ShowAction();
        }

        private void NotificationButton_Click(object sender, RoutedEventArgs e)
        {
            this.EditButton.IsChecked = false;
            this.parentWindow.ShowNotifications();
        }
    }
}
