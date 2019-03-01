using Net.Surviveplus.LightCutter.Commands;
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
    /// EditActionPage.xaml の相互作用ロジック
    /// </summary>
    public partial class EditActionPage : Page
    {
        public EditActionPage(MainWindow parentWindow, ActionPage actionPage, string accessText, string commands)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            this.actionPage = actionPage;
            this.originalCommand = commands;

            this.actionPreview.DataContext = new ActionViewModel
            {
                AccessText = accessText,
                DefaultShortcutVisibility = Visibility.Collapsed
            };

            if (string.IsNullOrWhiteSpace(commands))
            {
                commands = "Screen > Copy";
            }

            this.commandsTextBox.Text = commands;
            this.commandsTextBox.Focus();

            if(string.IsNullOrWhiteSpace( this.originalCommand))
            {
                this.deleteButton.Visibility = Visibility.Collapsed;
                this.Title.Content = "New Action";
            }
        }

        private string originalCommand;

        private MainWindow parentWindow;
        private ActionPage actionPage;

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.GoBack();

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var action = this.actionPreview.Tag as ActionCommands;
            if (action != null)
            {
                if(LightCutter.ActionCommands.ContainsKey(this.originalCommand))
                {
                    LightCutter.ActionCommands.Remove(this.originalCommand);
                }

                // TODO : key
                LightCutter.ActionCommands.Add(action.ToString(), action );
            } // end if


            if(this.actionPage != null)
            {
                this.actionPage.MustRefreshActions = true;
                this.parentWindow.GoBack();
            }
            else
            {
                this.parentWindow.ShowAction();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.GoBack();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: copy , Actions key  ...
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (LightCutter.ActionCommands.ContainsKey(this.originalCommand))
            {
                LightCutter.ActionCommands.Remove(this.originalCommand);
            }
            if (this.actionPage != null)
            {
                this.actionPage.MustRefreshActions = true;
                this.parentWindow.GoBack();
            }
            else
            {
                this.parentWindow.ShowAction();
            }
        }

        private void CommandsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var action = ActionCommands.FromCommands(this.commandsTextBox.Text);
                this.actionPreview.Content = action.DisplayCommand;
                this.actionPreview.ButtonIsEnabled = action.IsEnabled;
                this.actionPreview.Visibility = Visibility.Visible;
                this.commandError.Visibility = Visibility.Collapsed;
                this.actionPreview.Tag = action;
            }
            catch (Exception ex)
            {
                this.actionPreview.Tag = null;
                this.actionPreview.Visibility = Visibility.Collapsed;
                this.commandError.Visibility = Visibility.Visible;
                this.commandError.Text = $"({ex.Message})";
            } // end if

        }

        private void ActionPreview_Click(object sender, EventArgs e)
        {
            var action = this.actionPreview.Tag as ActionCommands;
            if (action != null) {
                using (new WindowHide(this.parentWindow))
                {
                    try
                    {
                        action.Do();    
                    }
                    catch
                    {

                        // TODO : Error
                    }
                } // end using
            } // end if
        }
    }
}
