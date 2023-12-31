/*
    @app        : StartAllBack Activator
    @repo       : https://github.com/Aetherinox/startallback-activator
    @author     : Aetherinox
*/

using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Text;
using Lng = SABActivator.Properties.Resources;
using Cfg = SABActivator.Properties.Settings;

namespace SABActivator
{
    class Patch
    {

        private static string path_base         = AppDomain.CurrentDomain.BaseDirectory;
        private static string path_exe          = System.IO.Path.GetDirectoryName( System.Reflection.Assembly.GetEntryAssembly( ).Location );
        private static string dll_target        = Cfg.Default.app_target_dll;

        private static string dll_appdata       = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ),
                                                    "StartAllBack",
                                                    dll_target
                                                );

        private static string dll_prog64        = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFiles ),
                                                    "StartAllBack",
                                                    dll_target
                                                );

        private static string dll_prog86        = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFilesX86 ),
                                                    "StartAllBack",
                                                    dll_target
                                                );

        private static string dll_exedir        = Path.Combine( path_exe,
                                                    "StartAllBack",
                                                    dll_target
                                                );

        /*
             Start Patch
        */

        public string Start( string path_selected = "auto" )
        {

            /*
                define arrays
            */

            string[] paths_arr      = new string[] { };
            string[] paths_lst      = new string[] { };

            /*
                populate path list array
            */

            Array.Resize( ref paths_lst, paths_lst.Length + 1 );
            paths_lst [ paths_lst.Length - 1 ] = dll_appdata;

            Array.Resize( ref paths_lst, paths_lst.Length + 1 );
            paths_lst [ paths_lst.Length - 1 ] = dll_prog64;

            Array.Resize( ref paths_lst, paths_lst.Length + 1 );
            paths_lst [ paths_lst.Length - 1 ] = dll_prog86;

            Array.Resize( ref paths_lst, paths_lst.Length + 1 );
            paths_lst [ paths_lst.Length - 1 ] = dll_exedir;

            /*
                define
            */

            if ( File.Exists( dll_appdata ) )
            {
                Array.Resize( ref paths_arr, paths_arr.Length + 1 );
                paths_arr [ paths_arr.Length - 1 ] = dll_appdata;
            }

            if ( File.Exists( dll_prog64 ) )
            {
                Array.Resize( ref paths_arr, paths_arr.Length + 1 );
                paths_arr [ paths_arr.Length - 1 ] = dll_prog64;
            }

            if ( File.Exists( dll_prog86 ) )
            {
                Array.Resize( ref paths_arr, paths_arr.Length + 1 );
                paths_arr [ paths_arr.Length - 1 ] = dll_prog86;
            }

            if ( File.Exists( dll_exedir ) )
            {
                Array.Resize( ref paths_arr, paths_arr.Length + 1 );
                paths_arr [ paths_arr.Length - 1 ] = dll_exedir;
            }

            /*
                custom file loaded
            */

            if ( path_selected != "auto" )
            {

                if ( File.Exists( path_selected ) )
                {

                    paths_arr      = new string[] { };
                    paths_lst      = new string[] { };

                    Array.Resize( ref paths_arr, paths_arr.Length + 1 );
                    paths_arr [ paths_arr.Length - 1 ] = path_selected;

                    Array.Resize( ref paths_lst, paths_lst.Length + 1 );
                    paths_lst [ paths_lst.Length - 1 ] = path_selected;

                    #if DEBUG
                        MessageBox.Show(
                            "DLL file was manually loaded\n\n" + path_selected,
                            "Debug: Custom DLL Loaded",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.None
                        );
                    #endif

                }
            }

            /*
                count results
            */

            int i_arr = paths_arr.Length;

            /*
                if list of paths empty, concat array into string and return error message listing what
                paths were checked.
            */

            if ( i_arr == 0 )
            {

                string path_compiled    = "";
                StringBuilder sb        = new StringBuilder( );

                sb.Append( Environment.NewLine );

                foreach ( string file in paths_lst )
                {
                    sb.Append( Environment.NewLine );
                    sb.Append( file );

                    path_compiled = sb.ToString( );
                }

                MessageBox.Show(
                    string.Format( Lng.msgbox_nolocpath_msg, path_compiled ),
                    Lng.msgbox_nolocpath_title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return Lng.statusbar_sab_not_found;
            }

            /*
                Temp kill automatic shell restart when process is killed
                AutoRestartShell = 0
            */

            RegistryKey ourKey  = Registry.LocalMachine;
            ourKey              = ourKey.OpenSubKey(
                                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon",
                                    true
                                );
            ourKey.SetValue     ( "AutoRestartShell", 0 );

            /*
                 kill task explorer.exe
             */

            try
            {
                Process[] processes = Process.GetProcessesByName( "explorer" );
                foreach ( Process proc in processes )
                {
                    proc.Kill( );
                }
            }
            catch ( Exception )
            {
                return Lng.statusbar_taskkill_proc;
            }

            /*
                 kill / restart task explorer.exe
             */

            try
            {
                Process.Start( "cmd", "/c taskkill /f /im explorer.exe" ).WaitForExit( );
            }
            catch ( Exception )
            {
                return Lng.statusbar_taskkill_cmd;
            }

            /*
                 kill task StartAllBackCfg.exe
             */

            try
            {
                Process[] processes = Process.GetProcessesByName( "StartAllBackCfg" );
                foreach ( Process proc in processes )
                {
                    proc.Kill( );
                }
            }
            catch ( Exception )
            {
                return Lng.statusbar_taskkill_startallback;
            }

            /*
                loop each dll path and modify the hex
            */

            foreach ( string path_dll in paths_arr )
            {
                string path_backup      = path_dll + ".bak";
                string query_takeown    = "takeown /f \'" + path_backup + "'";
                string query_icalcs     = "icacls \'\" + path_backup +\"' /grant *S-1-5-32-544:F /c /l";

                /*
                    backup file exists
                */

                if ( File.Exists( path_backup ) )
                {

                    /*
                        run powershell commands to adjust permissions
                    */

                    using ( PowerShell ps = PowerShell.Create( ) )
                    {

                        ps.AddScript( query_takeown );
                        ps.AddScript( query_icalcs );

                        Collection<PSObject> PSOutput   = ps.Invoke( );
                        StringBuilder sb                = new StringBuilder( );

                        foreach ( PSObject PSItem in PSOutput )
                        {
                            if ( PSItem != null )
                            {
                                Console.WriteLine( $"Output line: [{PSItem}]" );
                                sb.AppendLine( PSItem.ToString( ) );
                            }
                        }

                        if ( ps.Streams.Error.Count > 0 )
                        {
                            // Error collection
                        }
                    }

                    #if DEBUG
                        MessageBox.Show(
                            ".bak backup file already exists, deleting it and creating new\n\n" + path_backup,
                            "Debug: Found existing",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.None
                        );
                    #endif

                    File.Delete         ( path_dll );
                    File.Move           ( path_backup, path_dll );
                }

                /*
                    Patch
                */

                File.SetAttributes      ( path_dll, FileAttributes.Normal );
                File.Copy               ( path_dll, path_backup );
                File.SetAttributes      ( path_backup, FileAttributes.Normal );

                #if DEBUG
                    MessageBox.Show(
                        "The following file will be backed up: \n\n" + path_dll + "\n" + path_backup,
                        "Debug: Create Backup of DLL",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.None
                    );
                #endif

                byte[] bytes            = File.ReadAllBytes( path_dll );
                string hex_replace      = BitConverter.ToString( bytes ).Replace( "-", " " );
                string hex_result       = hex_replace.Replace(
                                            @"48 89 5C 24 08 55 56 57 48 8D AC 24 70 FF FF FF",
                                            @"67 C7 01 01 00 00 00 B8 01 00 00 00 C3 90 90 90"
                                        );

                #if DEBUG
                    try
                    {
                        StreamWriter sw_o = new StreamWriter( "hex_original.dmp" );
                        sw_o.WriteLine( hex_replace );
                        sw_o.Close( );

                        StreamWriter sw_p = new StreamWriter( "hex_patched.dmp" );
                        sw_p.WriteLine( hex_result );
                        sw_p.Close( );
                    }
                    catch ( Exception e )
                    {
                        Console.WriteLine( "Debug Hex Dump: Exception: " + e.Message );
                    }
                    finally
                    {
                        Console.WriteLine( "Debug Hex Dump: Finished" );
                    }
                #endif

                string[] hex_patch      = hex_result.Split(' ');
                byte[] bytes_modified   = new byte[ hex_patch.Length ];

                for ( int i = 0 ; i < hex_patch.Length ; i++ )
                {
                    bytes_modified [ i ] = Convert.ToByte( hex_patch [ i ], 16 );
                }

                File.WriteAllBytes( path_dll, bytes_modified );

                /*
                    launch StartAllBack
                */

                string path_sab_dir = Path.GetDirectoryName( path_dll );

                if ( !String.IsNullOrEmpty( path_sab_dir ) )
                {
                    string path_sab_exe = path_sab_dir + "\\" + Cfg.Default.app_target_exe;

                    if ( File.Exists( path_sab_exe ) )
                    {
                        System.Diagnostics.Process.Start( path_sab_exe );
                    }
                }
            }

            /*
                start task explorer.exe
            */

            Process.Start( "explorer" );

            /*
                re-enable AutoRestartShell in registry
                AutoRestartShell = 1
            */

            ourKey.SetValue( "AutoRestartShell", 1 );
            ourKey.Close( );

            MessageBox.Show(
                string.Format( Lng.msgbox_patch_compl_msg ),
                Lng.msgbox_patch_compl_title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            return Lng.statusbar_patch_complete;
        }
    }
}
