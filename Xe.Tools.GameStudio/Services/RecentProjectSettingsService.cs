using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.GameStudio.Services
{
    public static class RecentProjectSettingsService
    {
        public static void EnqueueRecentProject(string fileName)
        {
            Properties.Settings.Default.FileLastOpen = fileName;
            Properties.Settings.Default.Save();
        }
    }
}
