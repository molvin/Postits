using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

using System.Windows.Forms;

namespace PostIts
{
    public partial class PostItForm : Form
    {
        public int Id;
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;
        private const int IndentStep = 20;
        public SynchronizationContext Context { get; private set; }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        public PostItForm(int id, string rtf, bool select, Point? location, Size? size)
        {
            InitializeComponent();
            //Window style
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            SetStyle(ControlStyles.FixedWidth, true);
            TextBox.AcceptsTab = true;
            TextBox.DetectUrls = true;
            TextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            if (select)
                TextBox.Select();

            Activate();

            Context = SynchronizationContext.Current;
            ManagerForm.AddContextForm(Context, this);

            //Callbacks
            ExitButton.Click += (x,y) => Exit();
            NewWindowButton.Click += (x, y) => ManagerForm.NewWindow();
            SaveWindowButton.Click += (x,y) => SavesForm.OpenSavesForm();
            Move += (x, y) => Save();
            SizeChanged += (x, y) => Save();
            TextBox.KeyDown += TextBoxKeyDown;
            //Save data                   
            Id = id;
            TextBox.Rtf = rtf;
            if(location != null)
            {
                StartPosition = FormStartPosition.Manual;
                Location = location.Value;
            }
            if(size != null)
            {
                Size = size.Value;
            }
        }                                 
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(ClientSize.Width - cGrip, ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, BackColor, rc);
            rc = new Rectangle(0, 0, ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64,64,64)), rc);
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= ClientSize.Width - cGrip && pos.Y >= ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }
        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            Strike(e);
            HandleList(e);
            Save();
        }
        private void Strike(KeyEventArgs e)
        {
            if (e.KeyCode != Keys.K || !e.Control)
                return;
            
            if(TextBox.SelectionLength == 0)
            {
                int lineIndex = TextBox.GetLineFromCharIndex(TextBox.SelectionStart);
                int startIndex = 0;
                for(int i = 0; i < lineIndex; i++)
                {
                    startIndex += TextBox.Lines[i].Length + 1;
                }
                TextBox.Select(startIndex, TextBox.Lines[lineIndex].Length);
                Console.WriteLine(startIndex + " " + TextBox.SelectionStart);
            }

            TextBox.SelectionFont = new Font(TextBox.SelectionFont, TextBox.SelectionFont.Strikeout ? FontStyle.Regular : FontStyle.Strikeout);
            e.Handled = true;
            e.SuppressKeyPress = true;
            TextBox.Select(TextBox.SelectionStart + TextBox.SelectionLength, 0);
            TextBox.SelectionFont = new Font(TextBox.SelectionFont, FontStyle.Regular);       
        }
        private void HandleList(KeyEventArgs e)
        {
            if (TextBox.SelectionBullet)
            {
                if (e.KeyCode == Keys.Tab)
                    IndentList(e);
                else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Back)
                    BreakList(e);
            }
            else
            {
                if (e.Control && e.KeyCode == Keys.OemQuestion)
                    CreateList(e);
            }
        }
        private void IndentList(KeyEventArgs e)
        {
            //TODO: check for selection
            int lineIndex = TextBox.GetLineFromCharIndex(TextBox.SelectionStart);
            string line = (lineIndex < 0 || lineIndex >= TextBox.Lines.Length) ? "" : TextBox.Lines[lineIndex];

            bool fullSelect = TextBox.SelectionLength > 0 && TextBox.SelectionLength == line.Length;
            line = line.Trim();
            if (line.Length != 0 && !fullSelect)
                return;
            
            int indentLevel = Math.Max(TextBox.SelectionIndent / IndentStep + (e.Shift ? -1 : 1), 0);            
            TextBox.SelectionIndent = indentLevel * IndentStep;
            e.SuppressKeyPress = true;
            e.Handled = true;       
        }
        private void BreakList(KeyEventArgs e)
        {
            int lineIndex = TextBox.GetLineFromCharIndex(TextBox.SelectionStart);
            if (lineIndex < 0 || lineIndex >= TextBox.Lines.Length)
                return;

            if (TextBox.Lines[lineIndex].Length > 0)
                return;

            TextBox.SelectionBullet = false;
            TextBox.SelectionIndent = 0;
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
        private void CreateList(KeyEventArgs e)
        {
            TextBox.SelectionBullet = true;
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
        private void Save(bool open = true)
        {
            SaveManager.Save(Id, TextBox.Rtf, Location.X, Location.Y, Size.Width, Size.Height, open);
        }
        private void Exit()
        {
            ManagerForm.OnCloseForm(Id);
            Save(false);
            Close();
        }

    }
}