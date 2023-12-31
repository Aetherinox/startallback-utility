using System;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Lng = SABActivator.Properties.Resources;
using Cfg = SABActivator.Properties.Settings;

namespace SABActivator.Msgbox
{
    public partial class FormMessageBox : Form
    {
        //Fields
        private Color primaryColor = Color.CornflowerBlue;
        private int borderSize = 1;

        //Properties
        public Color PrimaryColor
        {
            get { return primaryColor; }
            set
            {
                primaryColor = value;
                this.BackColor = primaryColor;
                this.panelTitleBar.BackColor = PrimaryColor;
            }
        }

        //Constructors
        public FormMessageBox( string text )
        {
            InitializeComponent( );
            InitializeItems( );
            this.PrimaryColor = primaryColor;
            this.labelMessage.Text = text;
            this.labelCaption.Text = "";
            SetFormSize( );
            SetButtons( MessageBoxButtons.OK, MessageBoxDefaultButton.Button1 );
        }

        public FormMessageBox( string text, string caption )
        {
            InitializeComponent( );
            InitializeItems( );
            this.PrimaryColor           = primaryColor;
            this.labelMessage.Text      = text;
            this.labelCaption.Text      = caption;
            SetFormSize( );
            SetButtons( MessageBoxButtons.OK, MessageBoxDefaultButton.Button1 );
        }

        public FormMessageBox( string text, string caption, MessageBoxButtons buttons )
        {
            InitializeComponent( );
            InitializeItems( );
            this.PrimaryColor           = primaryColor;
            this.labelMessage.Text      = text;
            this.labelCaption.Text      = caption;
            SetFormSize( );
            SetButtons( buttons, MessageBoxDefaultButton.Button1 );
        }

        public FormMessageBox(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon
        )
        {
            InitializeComponent( );
            InitializeItems( );
            this.PrimaryColor           = primaryColor;
            this.labelMessage.Text      = text;
            this.labelCaption.Text      = caption;
            SetFormSize( );
            SetButtons( buttons, MessageBoxDefaultButton.Button1 );
            SetIcon( icon );
        }

        public FormMessageBox(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton btnDefault
        )
        {
            InitializeComponent( );
            InitializeItems( );
            this.PrimaryColor           = primaryColor;
            this.labelMessage.Text      = text;
            this.labelCaption.Text      = caption;
            SetFormSize( );
            SetButtons( buttons, btnDefault );
            SetIcon( icon );
        }

        //-> Private Methods
        private void InitializeItems( )
        {
            this.FormBorderStyle            = FormBorderStyle.None;
            this.Padding                    = new Padding( borderSize );
            this.labelMessage.MaximumSize   = new Size( 450, 0 );
            this.btnClose.DialogResult      = DialogResult.Cancel;
            this.button1.DialogResult       = DialogResult.OK;
            this.button1.Visible            = false;
            this.button2.Visible            = false;
            this.button3.Visible            = false;
        }

        private void SetFormSize( )
        {
            int widht =
                this.labelMessage.Width + this.pictureBoxIcon.Width + this.panelBody.Padding.Left;
            int height =
                this.panelTitleBar.Height
                + this.labelMessage.Height
                + this.panelButtons.Height
                + this.panelBody.Padding.Top;
            this.Size = new Size( widht, height );
        }

        private void SetButtons( MessageBoxButtons buttons, MessageBoxDefaultButton btnDefault )
        {
            int xCenter = (this.panelButtons.Width - button1.Width) / 2;
            int yCenter = (this.panelButtons.Height - button1.Height) / 2;

            switch ( buttons )
            {
                /*
                    BUTTON > OK
                */

                case MessageBoxButtons.OK:
                    button1.Visible = true;
                    button1.Location = new Point( xCenter, yCenter );
                    button1.Text = Lng.template_btn_ok;
                    button1.DialogResult = DialogResult.OK;

                    SetDefaultButton( btnDefault );
                    break;

                /*
                    BUTTON > OK / CANCEL
                */

                case MessageBoxButtons.OKCancel:

                    button1.Visible = true;
                    button1.Location = new Point( xCenter - ( button1.Width / 2 ) - 5, yCenter );
                    button1.Text = Lng.template_btn_ok;
                    button1.DialogResult = DialogResult.OK;

                    button2.Visible = true;
                    button2.Location = new Point( xCenter + ( button2.Width / 2 ) + 5, yCenter );
                    button2.Text = Lng.template_btn_cancel;
                    button2.DialogResult = DialogResult.Cancel;
                    button2.BackColor = Color.DimGray;

                    if ( btnDefault != MessageBoxDefaultButton.Button3 )
                        SetDefaultButton( btnDefault );
                    else
                        SetDefaultButton( MessageBoxDefaultButton.Button1 );
                    break;

                /*
                    BUTTON > RETRY / CANCEL
                */

                case MessageBoxButtons.RetryCancel:
                    button1.Visible = true;
                    button1.Location = new Point( xCenter - ( button1.Width / 2 ) - 5, yCenter );
                    button1.Text = Lng.template_btn_retry;
                    button1.DialogResult = DialogResult.Retry;

                    button2.Visible = true;
                    button2.Location = new Point( xCenter + ( button2.Width / 2 ) + 5, yCenter );
                    button2.Text = Lng.template_btn_cancel;
                    button2.DialogResult = DialogResult.Cancel;
                    button2.BackColor = Color.DimGray;

                    if ( btnDefault != MessageBoxDefaultButton.Button3 )
                        SetDefaultButton( btnDefault );
                    else
                        SetDefaultButton( MessageBoxDefaultButton.Button1 );
                    break;

                /*
                    BUTTON > YES / NO
                */

                case MessageBoxButtons.YesNo:
                    button1.Visible = true;
                    button1.Location = new Point( xCenter - ( button1.Width / 2 ) - 5, yCenter );
                    button1.Text = Lng.template_btn_yes;
                    button1.DialogResult = DialogResult.Yes;

                    button2.Visible = true;
                    button2.Location = new Point( xCenter + ( button2.Width / 2 ) + 5, yCenter );
                    button2.Text = Lng.template_btn_no;
                    button2.DialogResult = DialogResult.No;
                    button2.BackColor = Color.IndianRed;

                    if ( btnDefault != MessageBoxDefaultButton.Button3 )
                        SetDefaultButton( btnDefault );
                    else
                        SetDefaultButton( MessageBoxDefaultButton.Button1 );
                    break;

                /*
                    BUTTON > YES / NO / CANCEL
                */

                case MessageBoxButtons.YesNoCancel:
                    button1.Visible = true;
                    button1.Location = new Point( xCenter - button1.Width - 5, yCenter );
                    button1.Text = Lng.template_btn_yes;
                    button1.DialogResult = DialogResult.Yes;

                    button2.Visible = true;
                    button2.Location = new Point( xCenter, yCenter );
                    button2.Text = Lng.template_btn_no;
                    button2.DialogResult = DialogResult.No;
                    button2.BackColor = Color.IndianRed;

                    button3.Visible = true;
                    button3.Location = new Point( xCenter + button2.Width + 5, yCenter );
                    button3.Text = Lng.template_btn_cancel;
                    button3.DialogResult = DialogResult.Cancel;
                    button3.BackColor = Color.DimGray;

                    SetDefaultButton( btnDefault );
                    break;

                /*
                    BUTTON > ABORT / RETRY / IGNORE
                */

                case MessageBoxButtons.AbortRetryIgnore:
                    button1.Visible = true;
                    button1.Location = new Point( xCenter - button1.Width - 5, yCenter );
                    button1.Text = Lng.template_btn_abort;
                    button1.DialogResult = DialogResult.Abort;
                    button1.BackColor = Color.Goldenrod;

                    button2.Visible = true;
                    button2.Location = new Point( xCenter, yCenter );
                    button2.Text = Lng.template_btn_retry;
                    button2.DialogResult = DialogResult.Retry;

                    button3.Visible = true;
                    button3.Location = new Point( xCenter + button2.Width + 5, yCenter );
                    button3.Text = Lng.template_btn_ignore;
                    button3.DialogResult = DialogResult.Ignore;
                    button3.BackColor = Color.IndianRed;

                    SetDefaultButton( btnDefault );
                    break;
            }
        }

        /*
            Set Default Buttons
        */

        private void SetDefaultButton( MessageBoxDefaultButton btnDefault )
        {
            switch ( btnDefault )
            {
                /*
                    BUTTON > 1
                */

                case MessageBoxDefaultButton.Button1:
                    button1.Select( );
                    button1.ForeColor = Color.White;
                    button1.Font = new Font( button1.Font, FontStyle.Regular );
                    break;

                /*
                    BUTTON > 2
                */

                case MessageBoxDefaultButton.Button2:
                    button2.Select( );
                    button2.ForeColor = Color.White;
                    button2.Font = new Font( button2.Font, FontStyle.Regular );
                    break;

                /*
                    BUTTON > 3
                */

                case MessageBoxDefaultButton.Button3:
                    button3.Select( );
                    button3.ForeColor = Color.White;
                    button3.Font = new Font( button3.Font, FontStyle.Regular );
                    break;
            }
        }

        /*
            Set Icon
        */

        private void SetIcon( MessageBoxIcon icon )
        {
            switch ( icon )
            {
                /*
                    ERROR
                */

                case MessageBoxIcon.Error:
                    this.pictureBoxIcon.Image = Properties.Resources.error;
                    PrimaryColor = Color.FromArgb( 167, 15, 78 );
                    this.btnClose.FlatAppearance.MouseOverBackColor = Color.Transparent;
                break;

                /*
                    INFORMATION
                */

                case MessageBoxIcon.Information:
                    this.pictureBoxIcon.Image = Properties.Resources.information;
                    PrimaryColor = Color.FromArgb( 41, 108, 68 );
                    this.btnClose.FlatAppearance.MouseOverBackColor = Color.Transparent;
                break;

                /*
                    QUESTION
                */

                case MessageBoxIcon.Question:
                    this.pictureBoxIcon.Image = Properties.Resources.question;
                    PrimaryColor = Color.FromArgb( 51, 95, 129 );
                    this.btnClose.FlatAppearance.MouseOverBackColor = Color.Transparent;
                break;

                /*
                    EXCLAIMATION
                */

                case MessageBoxIcon.Exclamation:
                    this.pictureBoxIcon.Image = Properties.Resources.exclamation;
                    PrimaryColor = Color.FromArgb( 169, 111, 40 );
                    this.btnClose.FlatAppearance.MouseOverBackColor = Color.Transparent;
                break;

                /*
                    NONE
                */

                case MessageBoxIcon.None:
                    this.pictureBoxIcon.Image = Properties.Resources.chat;
                    PrimaryColor = Color.FromArgb( 45, 45, 45 );
                    this.btnClose.FlatAppearance.MouseOverBackColor = Color.Transparent;
                break;

            }
        }

        //-> Events Methods
        private void btnClose_Click( object sender, EventArgs e )
        {
            this.Close( );
        }

        /*
            Window > Button > Close > Mouse Enter
        */

        private void btnClose_MouseEnter( object sender, EventArgs e )
        {
            this.btnClose.ForeColor = Color.FromArgb( 222, 31, 100 );
        }

        /*
            Window > Button > Close > Mouse Leave
        */

        private void btnClose_MouseLeave( object sender, EventArgs e )
        {
            this.btnClose.ForeColor = Color.FromArgb( 255, 255, 255 );
        }


        #region -> Drag Form
        [DllImport( "user32.DLL", EntryPoint = "SendMessage" )]
        private static extern void SendMessage(
            System.IntPtr hWnd,
            int wMsg,
            int wParam,
            int lParam
        );

        [DllImport( "user32.DLL", EntryPoint = "ReleaseCapture" )]
        private static extern void ReleaseCapture( );

        private void panelTitleBar_MouseDown( object sender, MouseEventArgs e )
        {
            ReleaseCapture( );
            SendMessage( this.Handle, 0x112, 0xf012, 0 );
        }
        #endregion

        private void lblMessage_Click( object sender, EventArgs e ) { }

        private void panelButtons_Paint( object sender, PaintEventArgs e )
        {

        }

        private void panelButtons_Paint_1( object sender, PaintEventArgs e )
        {

        }

        private void FormMessageBox_Load( object sender, EventArgs e )
        {

        }
    }
}
