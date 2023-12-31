using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

/*

    Aetherx > Control > Button

    Button customization

*/

namespace SABActivator
{

    public class AetherxButton : Button
    {

        /*
            Show keyboard cues no matter what.
            By default, user must press ALT to see them.
        */

        protected override bool ShowKeyboardCues
        {
            get
            {
                return true;
            }
        }
    }
}
