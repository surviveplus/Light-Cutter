﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Targeting
{
    public class TargetRemoteDesktopConnectionCommand : IActionCommand
    {
        public static TargetRemoteDesktopConnectionCommand FromCommand(string command)
        {
            if (command == "Remote Desktop") { return new TargetRemoteDesktopConnectionCommand(); }
            return null;
        }

        public string Command => "Remote Desktop";

        public IEnumerable<object> DisplayCommand => ActionCommandDisplay.Create(new UI.Parts.RemoteDesktop(), " Remote Desktop");

        public bool IsEnabled => true;

        public bool MustUac => false;

        public void Do(ActionState state)
        {
            if (state.IsCanceled) return;

            Debug.WriteLine($"{this.Command}");

            var bitmap = PrintWindowImage.PrintWindow("{TscShellContainerClass}", "{UIMainClass}", "Remote Desktop Connection");
            state.CroppedImage = new Core.CroppedImage(bitmap);


        }
    }
}
