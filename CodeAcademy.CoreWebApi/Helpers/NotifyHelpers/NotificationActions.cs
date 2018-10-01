using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers.NotifyHelpers
{
    public static class NotificationActions
    {
        public static string Added { get; } = "Added";

        public static string Approved { get; } = "Approved";

        public static string Dispproved { get; } = "Dispproved";

        public static string Reply { get; } = "Reply";  

        public static string Like { get; } = "Like";

    }
}
