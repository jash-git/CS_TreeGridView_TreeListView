// ♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉
// <copyright company="brightman software studios" file="Program.cs">
//   Copyright © brightman software studios 2008-2012. All rights reserved.
// </copyright>
// <author>Peter Brightman</author>
// ♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉♉

using System;
using System.Windows.Forms;
using VirtualModeTreeListView.Model;

namespace VirtualModeTreeListView
{
    static class Program
    {
        /// <summary>
        /// entry point
        /// </summary>
        [STAThread]
        static void Main()
        {
            IModel mModel = new Model.Model();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(mModel));
        }
    }
}