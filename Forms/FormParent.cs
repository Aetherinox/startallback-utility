/*
    @app        : StartAllBack Activator
    @repo       : https://github.com/Aetherinox/startallback-utility
    @author     : Aetherinox
*/

using SABActivator;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static SABActivator.FormParent;
using System.Text;
using System.Text.RegularExpressions;
using Lng = SABActivator.Properties.Resources;
using Cfg = SABActivator.Properties.Settings;

namespace SABActivator
{
    public partial class FormParent : Form
    {

        /*
            Classes
        */

        private Patch Serial        = new Patch( );
        private Helpers Helpers     = new Helpers( );

        /*
            variables > query
        */

        private string query_result;

        /*
            Frame > Parent
        */

        public FormParent( )
        {
            InitializeComponent( );

            this.statusStrip.Renderer   = new StatusBar_Renderer( );
            string product              = AppInfo.Title;
            lblTitle.Text               = product;
            txt_intro.Text              = string.Format( Lng.txt_intro, Environment.NewLine );
            btnGenerate.Text            = Lng.btn_patch;
            btnManualDLL.Text           = Lng.btn_locate;

            string l1                   = Lng.txt_intro1;
            string l2                   = Lng.txt_intro2;
            string l3                   = Lng.txt_intro3;

            rtxt_Intro.Text             = "";

            rtxt_Intro.AppendText       ( l1 );
            rtxt_Intro.Select           ( 0, l1.Length );
            rtxt_Intro.SelectionColor   = Color.White;

            rtxt_Intro.AppendText       ( "\n\n" );

            rtxt_Intro.AppendText       ( l2 );
            rtxt_Intro.Select           ( l1.Length + 1, l2.Length );
            rtxt_Intro.SelectionColor   = Color.FromArgb( 31, 133, 222 );

            rtxt_Intro.AppendText       ( "\n" );

            rtxt_Intro.AppendText       ( l3 );
            rtxt_Intro.Select           ( l1.Length + 1 + l2.Length + 1, l3.Length );
            rtxt_Intro.SelectionColor   = Color.FromArgb( 189, 164, 49 );

        }

        /*
            Frame > Parent > Load
        */

        private void FormParent_Load( object sender, EventArgs e )
        {
            mnuTop.Renderer             = new ToolStripProfessionalRenderer( new mnuTop_ColorTable( ) );
            toolStripStatusLabel1.Text  = string.Format( Lng.statusbar_generate );
            statusStrip.Refresh( );
        }

        /*
            Window > Button > Minimize > Click
        */

        private void btn_Window_Minimize_Click( object sender, EventArgs e )
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /*
            Window > Button > Minimize > Mouse Enter
        */

        private void btn_Window_Minimize_MouseEnter( object sender, EventArgs e )
        {
            minimizeBtn.ForeColor = Color.FromArgb( 222, 31, 100 );
        }

        /*
            Window > Button > Minimize > Mouse Leave
        */

        private void btn_Window_Minimize_MouseLeave( object sender, EventArgs e )
        {
            minimizeBtn.ForeColor = Color.FromArgb( 255, 255, 255 );
        }

        /*
            Window > Button > Close > Click
        */

        private void btn_Window_Close_Click( object sender, EventArgs e )
        {
            Application.Exit( );
        }

        /*
            Window > Button > Close > Mouse Enter
        */

        private void btn_Window_Close_MouseEnter( object sender, EventArgs e )
        {
            closeBtn.ForeColor = Color.FromArgb( 222, 31, 100 );
        }

        /*
            Window > Button > Close > Mouse Leave
        */

        private void btn_Window_Close_MouseLeave( object sender, EventArgs e )
        {
            closeBtn.ForeColor = Color.FromArgb( 255, 255, 255 );
        }

        /*
            Main Form > Mouse Down
            deals with moving form around on screen
        */

        private bool mouseDown;
        private Point lastLocation;

        private void MainForm_MouseDown( object sender, MouseEventArgs e )
        {
            mouseDown       = true;
            lastLocation    = e.Location;
        }

        /*
            Main Form > Mouse Up
            deals with moving form around on screen
        */

        private void MainForm_MouseUp( object sender, MouseEventArgs e )
        {
            mouseDown       = false;
        }

        /*
            Main Form > Mouse Move
            deals with moving form around on screen
        */

        private void MainForm_MouseMove( object sender, MouseEventArgs e )
        {
            if ( mouseDown )
            {
                this.Location = new Point(
                    ( this.Location.X - lastLocation.X ) + e.X,
                    ( this.Location.Y - lastLocation.Y ) + e.Y
                );

                this.Update( );
            }
        }

        /*
            Label > Window Title
        */

        private void lbl_Title_Click( object sender, EventArgs e ) { }

        /*
            Top Menu > File > Exit
        */

        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Application.Exit( );
        }

        /*
            Top Menu > Help > About
        */

        private void aboutToolStripMenuItem1_Click( object sender, EventArgs e )
        {
            FormAbout to    = new FormAbout( );
            to.TopMost      = true;
            to.Show( );
        }

        /*
            Top Menu > Click Item
        */

        private void mnuTop_ItemClicked( object sender, ToolStripItemClickedEventArgs e ) { }

        /*
            Status Bar > Color Table
        */

        public class ClrTable : ProfessionalColorTable
        {
            public override Color StatusStripGradientBegin => Color.FromArgb( 35, 35, 35 );
            public override Color StatusStripGradientEnd => Color.FromArgb( 35, 35, 35 );
        }

        /*
            Status Bar > Renderer
            Override colors
        */

        public class StatusBar_Renderer : ToolStripProfessionalRenderer
        {
            public StatusBar_Renderer( )
                : base( new ClrTable( ) ) { }

            protected override void OnRenderToolStripBorder( ToolStripRenderEventArgs e )
            {
                if ( !( e.ToolStrip is StatusStrip ) )
                    base.OnRenderToolStripBorder( e );
            }
        }

        /*
            Top Menu > Override Render Colors
        */

        public class mnuTop_ColorTable : ProfessionalColorTable
        {
            /*
                Gets the starting color of the gradient used when
                a top-level System.Windows.Forms.ToolStripMenuItem is pressed.
            */

            public override Color MenuItemPressedGradientBegin => Color.FromArgb( 55, 55, 55 );

            /*
                Gets the end color of the gradient used when a top-level
                System.Windows.Forms.ToolStripMenuItem is pressed.
            */

            public override Color MenuItemPressedGradientEnd => Color.FromArgb( 55, 55, 55 );

            /*
                Gets the border color to use with a
                System.Windows.Forms.ToolStripMenuItem.
            */

            public override Color MenuItemBorder => Color.FromArgb( 0, 45, 45, 45 );

            /*
                Gets the starting color of the gradient used when the
                System.Windows.Forms.ToolStripMenuItem is selected.
            */

            public override Color MenuItemSelectedGradientBegin => Color.FromArgb( 222, 31, 103 );

            /*
                Gets the end color of the gradient used when the
                System.Windows.Forms.ToolStripMenuItem is selected.
            */

            public override Color MenuItemSelectedGradientEnd => Color.FromArgb( 222, 31, 103 );

            /*
                Gets the solid background color of the
                System.Windows.Forms.ToolStripDropDown.
            */

            public override Color ToolStripDropDownBackground => Color.FromArgb( 40, 40, 40 );

            /*
                Top Menu > Image > Start Gradient Color
            */

            public override Color ImageMarginGradientBegin => Color.FromArgb( 222, 31, 103 );

            /*
                Top Menu > Image > Middle Gradient Color
            */

            public override Color ImageMarginGradientMiddle => Color.FromArgb( 222, 31, 103 );

            /*
                Top Menu > Image > End Gradient Color
            */

            public override Color ImageMarginGradientEnd => Color.FromArgb( 222, 31, 103 );

            /*
                Top Menu > Shadow Effect
            */

            public override Color SeparatorDark => Color.FromArgb( 0, 45, 45, 45 );

            /*
                Top Menu > Border Color
            */

            public override Color MenuBorder => Color.FromArgb( 0, 45, 45, 45 );

            /*
                 Top Menu > Item Hover BG Color
             */

            public override Color MenuItemSelected => Color.FromArgb( 222, 31, 103 );
        }

        private void statusStrip_ItemClicked( object sender, ToolStripItemClickedEventArgs e ) { }

        /*
            Label > Sub Header
                "Generated License key"
        */

        private void lbl_Serial_Sub_Click( object sender, EventArgs e ) { }

        /*
            Label > Body
                "Click the "Start" button below to create a number number."
        */

        private void lbl_LicenseKey_Click( object sender, EventArgs e ) { }

        private void txt_User__TextChanged( object sender, EventArgs e )
        {

        }

        /*
            button > Start > OnClick
        */

        private void btn_Generate_Click( object sender, EventArgs e )
        {
            query_result = Serial.Start( );

            toolStripStatusLabel1.Text = string.Format( query_result );
            statusStrip.Refresh( );
        }

        private void btnOpenFolder_Click( object sender, EventArgs e )
        {
            string src_file_path        = Helpers.FindApp( );

            if ( String.IsNullOrEmpty( src_file_path ) )
            {
                MessageBox.Show(
                    string.Format( Lng.msgbox_nolocopen_msg ),
                    Lng.msgbox_nolocopen_title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            /*
                target directory
            */

            if ( Directory.Exists( src_file_path ) )
                Process.Start( "explorer.exe", src_file_path );

            /*
                cannot locate mobaxterm program. Open dialog in Program Files(86)
            */

            else
            {
                string path_progfiles = Helpers.ProgramFiles( );
                Process.Start( "explorer.exe", path_progfiles );

                MessageBox.Show(
                    string.Format( Lng.msgbox_nolocopen_msg ),
                    Lng.msgbox_nolocopen_title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

        }

        private void btnFind_Click( object sender, EventArgs e )
        {

            string src_file_path    = Helpers.FindApp( );
            string ext_default      = @"c:\";

            if ( !String.IsNullOrEmpty( src_file_path ) )
            {
                ext_default         = src_file_path;
            }

            OpenFileDialog dlg      = new OpenFileDialog( );
            dlg.Title               = "Open DLL file";
            dlg.InitialDirectory    = ext_default;
            dlg.Filter              = "SAB Library DLL|StartAllBackX64.dll|All files (*.*)|*.*";

            if ( dlg.ShowDialog( ) == DialogResult.OK )
            {
                StreamReader sr     = File.OpenText(dlg.FileName);

                string s            = sr.ReadLine();
                StringBuilder sb    = new StringBuilder();

                while ( s != null )
                {
                    sb.Append( s );
                    s = sr.ReadLine( );
                }
                sr.Close( );

                toolStripStatusLabel1.Text = string.Format( "Loaded DLL from " + dlg.FileName );
                statusStrip.Refresh( );

                foreach ( String file in dlg.FileNames )
                {
                    query_result = Serial.Start( file );

                    toolStripStatusLabel1.Text = string.Format( query_result );
                    statusStrip.Refresh( );
                }

            }
        }

        private void githubUpdatesToolStripMenuItem_Click( object sender, EventArgs e )
        {
            System.Diagnostics.Process.Start( Cfg.Default.app_url_github );
        }

        private void lblTitle_MouseDown( object sender, MouseEventArgs e )
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void lblTitle_MouseUp( object sender, MouseEventArgs e )
        {
            mouseDown = false;
        }

        private void lblTitle_MouseMove( object sender, MouseEventArgs e )
        {
            if ( mouseDown )
            {
                this.Location = new Point(
                    ( this.Location.X - lastLocation.X ) + e.X,
                    ( this.Location.Y - lastLocation.Y ) + e.Y
                );

                this.Update( );
            }
        }

        private void statusStrip_MouseDown( object sender, MouseEventArgs e )
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void statusStrip_MouseUp( object sender, MouseEventArgs e )
        {
            mouseDown = false;
        }

        private void statusStrip_MouseMove( object sender, MouseEventArgs e )
        {
            if ( mouseDown )
            {
                this.Location = new Point(
                    ( this.Location.X - lastLocation.X ) + e.X,
                    ( this.Location.Y - lastLocation.Y ) + e.Y
                );

                this.Update( );
            }
        }
    }
}
