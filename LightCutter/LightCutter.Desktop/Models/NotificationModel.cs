using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Desktop.Models
{
    public class NotificationModel
    {
        public static NotificationModel FromException(Exception ex, string failedActionDisplayName) {
            var m = new NotificationModel {
                Message = ex.Message,
                Title = $"{failedActionDisplayName} is failed. "};
            return m;
        }

        public string Title { get; set; }
        public string Message { get; set; }
    }
}
