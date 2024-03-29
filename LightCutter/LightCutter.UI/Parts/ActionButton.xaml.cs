﻿using Net.Surviveplus.LightCutter.UI.ViewModels;
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

namespace Net.Surviveplus.LightCutter.UI.Parts
{
    /// <summary>
    /// ActionButton.xaml の相互作用ロジック
    /// </summary>
    public partial class ActionButton : UserControl
    {
        public ActionButton()
        {
            InitializeComponent();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.RefreshDefaultSelectionArea();
            this.RefreshMainButtonEnabled();
            this.RefreshUac();

        } // end sub

        public event EventHandler<EventArgs> Click;


        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            this.Click?.Invoke(this, EventArgs.Empty);
        } // end sub

        /// <summary>
        /// Backing field of ButtonIsEnabled property.
        /// </summary>
        private bool valueOfButtonIsEnabled = true;

        /// <summary>
        /// Gets or sets Button.IsEnabled.
        /// </summary>
        public bool ButtonIsEnabled
        {
            get
            {
                return this.valueOfButtonIsEnabled;
            } // end get
            set
            {
                this.valueOfButtonIsEnabled = value;
                RefreshMainButtonEnabled();

            } // end set
        } // end property

        private void RefreshMainButtonEnabled()
        {
            var thisMainButton = this.Template.FindName("MainButton", this) as Button;
            if (thisMainButton != null) thisMainButton.IsEnabled = this.valueOfButtonIsEnabled;
        }


        /// <summary>
        /// Backing field of ShowDefaultActionSelection property.
        /// </summary>
        private bool valueOfShowDefaultActionSelection;

        /// <summary>
        /// Gets or sets the value of something.
        /// </summary>
        public bool ShowDefaultActionSelection
        {
            get
            {
                return this.valueOfShowDefaultActionSelection;
            } // end get
            set
            {
                this.valueOfShowDefaultActionSelection = value;

                RefreshDefaultSelectionArea();

            } // end set
        } // end property

        private void RefreshDefaultSelectionArea()
        {
            var defaultSelectionArea = this.Template.FindName("defaultSelectionArea", this) as ColumnDefinition;
            if (defaultSelectionArea != null) defaultSelectionArea.Width = this.valueOfShowDefaultActionSelection ? new GridLength(20) : new GridLength(0);

            var thisIsDefaultRadio = this.Template.FindName("IsDefaultRadio", this) as RadioButton;
            if (thisIsDefaultRadio != null) thisIsDefaultRadio.IsChecked = (this.DataContext as ActionViewModel)?.DefaultShortcutVisibility == Visibility.Visible;

            var buttonArea = this.Template.FindName("buttonArea", this) as ColumnDefinition;
            if (buttonArea != null) buttonArea.Width = this.valueOfShowDefaultActionSelection ? new GridLength(40) : new GridLength(0);

        }

        private void IsDefaultRadio_Checked(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ActionViewModel).DefaultShortcutVisibility = Visibility.Visible;
            this.IsDefaultChanged?.Invoke(this, EventArgs.Empty);
        }

        private void IsDefaultRadio_Unchecked(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ActionViewModel).DefaultShortcutVisibility = Visibility.Collapsed;
            this.IsDefaultChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> IsDefaultChanged;

        private void EditActionButton_Click(object sender, RoutedEventArgs e)
        {
            this.EditButtonClick?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> EditButtonClick;

        private bool valueOfMustUac;

        public bool MustUac { get => valueOfMustUac;
            set {
                valueOfMustUac = value;
                this.RefreshUac();
            }
        }
        private void RefreshUac()
        {
            var uac = this.Template.FindName("uac", this) as Label;

            if(uac != null)
            {
                if (this.valueOfMustUac)
                {
                    if(uac.Content == null)
                    {
                        uac.Content = new UI.Parts.Uac();
                    }
                }
                else
                {
                    uac.Content = null;
                }

            } // end if
        }

    } // end class
} // end namespace

