using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SABActivator
{

    static class Program
    {
        [STAThread]
        static void Main( )
        {
            Application.EnableVisualStyles( );
            Application.SetCompatibleTextRenderingDefault(false);

            /*
                 Elevate to admin so we can modify the windows host file.
            */

            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent( ));
            bool administrativeMode = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!administrativeMode)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo( );
                startInfo.Verb = "runas";
                startInfo.FileName = Application.ExecutablePath;
                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                    return;
                }
                return;
            }

            Application.Run(new FormParent( ));

        }

    }
}
