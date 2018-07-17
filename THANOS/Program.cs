﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.Extensions;
using Engine.ProcessCore;
using Engine.UWP;

namespace THANOS
{
    class Program
    {
        public static Core TargetProcess;
        public static bool Verbose;

        [STAThread]
        static void Main(string[] args)
        {
            Verbose = false;

            if (args != null && args.Length > 0)
            {
                if (args[0].StartsWith("verbose="))
                    Verbose = args[0].Replace("verbose=", "").ParseFromString();
            }

            if (Helper.IsRunningElevated())
            {
                Tokenizer.Initiate();
                Tokenizer.SetProcessDebugToken((int)WinAPI.GetCurrentProcessId());
                Tokenizer.ImpersonateSystem();
                //Tokenizer.ImpersonateTrustedInstaller();
                using (frmProcesses frm = new frmProcesses())
                {
                    frm.ShowDialog();
                    if (TargetProcess != null)
                    {
                        using (frmInjection fi = new frmInjection(TargetProcess))
                        {
                            fi.ShowDialog();
                        }
                    }
                }
            }
        }
    }
}
