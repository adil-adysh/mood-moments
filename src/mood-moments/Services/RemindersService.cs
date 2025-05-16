using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using Plugin.LocalNotification.EventArgs;

namespace mood_moments.Services
{
    public class RemindersService
    {
        public void ScheduleReminder(string text, DateTime time, int id)
        {
            var notification = new NotificationRequest
            {
                NotificationId = id,
                Title = "Mood Moments Reminder",
                Description = text,
                ReturningData = "reminder",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = time
                }
            };

            // ✅ Use the correct class for Plugin.LocalNotification v12.x
            Plugin.LocalNotification.LocalNotificationCenter.Current.Show(notification);
        }
    }
}
