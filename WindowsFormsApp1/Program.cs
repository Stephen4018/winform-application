using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormApp1;
using WindowsFormsApp1.Implementation;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var serviceProvider = new ServiceCollection()
                .AddTransient<ICommandParser, CommandParser>()
                .AddTransient<Form1>()
                .BuildServiceProvider();

            var form = serviceProvider.GetRequiredService<Form1>();

            Application.Run(form);
        }
    }
}
