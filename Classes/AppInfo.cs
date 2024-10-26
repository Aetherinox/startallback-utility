/*
    @app        : StartAllBack Activator
    @repo       : https://github.com/Aetherinox/startallback-utility
    @author     : Aetherinox
*/

using System;
using System.Reflection;

namespace SABActivator
{

    /*
         AppInfo
            returns title, copyright, version, etc of Application.
    */

    class AppInfo
    {

        /*
            AppInfo -> Title
        */

        public static string Title
        {
            get
            {
                AssemblyTitleAttribute title = (AssemblyTitleAttribute)Assembly.GetExecutingAssembly( ).GetCustomAttribute(typeof(AssemblyTitleAttribute));

                if (title != null && !string.IsNullOrEmpty(title.Title))
                    return title.Title;

                return string.Empty;
            }
        }

        /*
            AppInfo -> Description
        */

        public static string Description
        {
            get
            {
                AssemblyDescriptionAttribute desc = (AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly( ).GetCustomAttribute(typeof(AssemblyDescriptionAttribute));

                if (desc != null && !string.IsNullOrEmpty(desc.Description))
                    return desc.Description;

                return string.Empty;
            }
        }

        /*
            AppInfo -> Author
        */

        public static string Trademark
        {
            get
            {
                AssemblyTrademarkAttribute tm = (AssemblyTrademarkAttribute)Assembly.GetExecutingAssembly( ).GetCustomAttribute(typeof(AssemblyTrademarkAttribute));

                if (tm != null && !string.IsNullOrEmpty(tm.Trademark))
                    return tm.Trademark;

                return string.Empty;
            }
        }

        /*
            AppInfo -> Company
        */

        public static string Company
        {
            get
            {
                AssemblyCompanyAttribute comp = (AssemblyCompanyAttribute)Assembly.GetExecutingAssembly( ).GetCustomAttribute(typeof(AssemblyCompanyAttribute));

                if (comp != null && !string.IsNullOrEmpty(comp.Company))
                    return comp.Company;

                return string.Empty;
            }
        }

        /*
            AppInfo -> Copyright
        */

        public static string Copyright
        {
            get
            {
                AssemblyCopyrightAttribute cr = (AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly( ).GetCustomAttribute(typeof(AssemblyCopyrightAttribute));

                if (cr != null && !string.IsNullOrEmpty(cr.Copyright))
                    return cr.Copyright;

                return string.Empty;
            }
        }

        /*
            AppInfo -> Version
        */

        public static string Version
        {
            get
            {
                Version _ver        = Assembly.GetExecutingAssembly( ).GetName( ).Version;
                string ver          = _ver.Major + "." + _ver.Minor + "." + _ver.Build + "." + _ver.Revision;

                if ( ver != null && !string.IsNullOrEmpty( ver ) )
                    return ver.ToString( );

                return string.Empty;
            }
        }

        public static string ProductVersionCore
        {
            get
            {
                return System.Windows.Forms.Application.ProductVersion;
            }
        }

        public static string PublishVersion
        {
            get
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    Version ver = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    return string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
                }
                else
                {
                    var ver = Assembly.GetExecutingAssembly( ).GetName( ).Version;
                    return string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
                }
            }
        }
    }
}