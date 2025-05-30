﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Helpers.FormManager;

namespace PIA_MAD_FyD
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = FormManager.ShowForm<Form1>();
            Application.Run(form);
        }
    }
}
