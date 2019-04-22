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
        public EditActionPage(MainWindow parentWindow, ActionPage actionPage, string accessText, Models.ActionModel originalAction )
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            this.actionPage = actionPage;
            this.originalAction = originalAction;

            this.actionPreview.DataContext = new ActionViewModel
            {
                AccessText = accessText,
                DefaultShortcutVisibility = Visibility.Collapsed
            };

            var commands = originalAction?.Commands;
            if (string.IsNullOrWhiteSpace(commands))
            {
                commands = "Screen > Copy";
            }

            this.commandsTextBox.Text = commands;
            this.commandsTextBox.SelectAll();
            this.commandsTextBox.Focus();

            if(originalAction == null)
            {
                this.deleteButton.Visibility = Visibility.Collapsed;
                this.copyButton.Visibility = Visibility.Collapsed;
                this.Title.Content = "New Action";
            }
        }

        private Models.ActionModel originalAction;
        private MainWindow parentWindow;
        private ActionPage actionPage;

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.GoBack();

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var replaced = false;
            var action = this.actionPreview.Tag as ActionCommands;
            if (action != null )
            {
                var newCommand = action.ToString();
                if(this.originalAction?.Commands != newCommand)
                {
                    LightCutter.ReplaceOrAddAction(this.originalAction, newCommand);
                    replaced = true;
                } // end if
            } // end if

            if(this.actionPage != null)
            {
                this.actionPage.MustRefreshActions = replaced;
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
            var added = false;
            var action = this.actionPreview.Tag as ActionCommands;
            if (action != null)
            {
                LightCutter.AddAction(action.ToString());
                added = true;
            } // end if

            if (this.actionPage != null)
            {
                this.actionPage.MustRefreshActions = added;
                this.parentWindow.GoBack();
            }
            else
            {
                this.parentWindow.ShowAction();
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            LightCutter.RemoveAction(this.originalAction);

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
                this.actionPreview.MustUac = action.MustUac;
                this.actionPreview.Visibility = Visibility.Visible;
                this.commandError.Visibility = Visibility.Collapsed;
                this.runningError.Visibility = Visibility.Collapsed;
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
            this.runningError.Visibility = Visibility.Collapsed;
            var action = this.actionPreview.Tag as ActionCommands;
            if (action != null) {
                using (new WindowHide(this.parentWindow))
                {
                    try
                    {
                        action.Do();    
                    }
                    catch(Exception ex)
                    {
                        this.runningError.Text = ex.Message;
                        this.runningError.Visibility = Visibility.Visible;
                    }
                } // end using
            } // end if
        }

        private void CommandButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var command = button?.Tag as string;
            if(!string.IsNullOrWhiteSpace( command))
            {
                this.commandsTextBox.SelectedText = command;
                var start = this.commandsTextBox.SelectionStart + this.commandsTextBox.SelectionLength;
                this.commandsTextBox.SelectionLength = 0;
                this.commandsTextBox.SelectionStart = start;
                this.commandsTextBox.Focus();
            } // end if
        }

        private void HelpLink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/surviveplus/Light-Cutter/blob/master/HowToUse/HowToEditAction.md");

        }
    }
}
