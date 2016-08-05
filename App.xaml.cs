using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace a7JsonViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ArgFilePath { get; private set; }

        //Add this method override
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args?.Length > 0)
            {
                ArgFilePath = e.Args[0];
            }
        }
    }
}
