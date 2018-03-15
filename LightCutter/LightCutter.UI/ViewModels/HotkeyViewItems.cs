using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Net.Surviveplus.LightCutter.UI.ViewModels
{
    public static class HotkeyViewItems
    {
        public static List<HotkeyViewModel> CreateAll()
        {

            var items = new List<HotkeyViewModel>();

            var k = HotKeys.F2;
            while (k != HotKeys.None)
            {

                items.Add(new HotkeyViewModel
                {
                    Caption = "Win + " + k,
                    Win = Visibility.Visible,
                    Key = k
                });

                k = k.Next();
            }

            k = HotKeys.A;
            while (k != HotKeys.None)
            {

                switch (k)
                {
                    case HotKeys.C:
                    case HotKeys.M:
                    case HotKeys.N:
                    case HotKeys.P:
                    case HotKeys.S:
                    case HotKeys.T:
                    case HotKeys.W:
                        break;

                    default:
                        items.Add(new HotkeyViewModel
                        {
                            Caption = "Win + Shift + " + k,
                            Win = Visibility.Visible,
                            Shift = Visibility.Visible,
                            Key = k
                        });
                        break;
                }



                k = k.Next();
            }

            k = HotKeys.F1;
            while (k != HotKeys.None)
            {

                items.Add(new HotkeyViewModel
                {
                    Caption = "Ctrl + Shift + " + k,
                    Ctrl = Visibility.Visible,
                    Shift = Visibility.Visible,
                    Key = k
                });

                k = k.Next();
            }

            k = HotKeys.A;
            while (k != HotKeys.None)
            {

                items.Add(new HotkeyViewModel
                {
                    Caption = "Ctrl + Shift + Alt + " + k,
                    Ctrl = Visibility.Visible,
                    Shift = Visibility.Visible,
                    Alt = Visibility.Visible,
                    Key = k
                });

                k = k.Next();
            }

            return items;
        } // end function
    }
}
