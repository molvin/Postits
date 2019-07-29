using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace PostIts
{
    public partial class PostItForm : Form
    {
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;
        private int indentLevel = 0;
        private const int IndentStep = 20;
        private bool strike = false;

        public PostItForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            SetStyle(ControlStyles.FixedWidth, true);

            ExitButton.Click += Exit;
            NewWindowButton.Click += NewWindow;

            TextBox.KeyDown += TextBoxKeyDown;

            TextBox.AcceptsTab = true;
            TextBox.DetectUrls = true;
            TextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            TextBox.Select();
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
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void Exit(object sender, EventArgs e)
        {
            Close();
        }
        private void NewWindow(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Application.Run(new PostItForm()));
            thread.Start();
        }

        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.K && e.Control)
            {
                TextBox.SelectionFont = new Font(TextBox.SelectionFont, FontStyle.Strikeout);
                e.Handled = true;
                e.SuppressKeyPress = true;
                strike = true;
                return;
            }
            {
                strike = false;
                TextBox.SelectionFont = new Font(TextBox.SelectionFont, FontStyle.Regular);
            }

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
            
            indentLevel = Math.Max(indentLevel + (e.Shift ? -1 : 1), 0);            
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
            indentLevel = 0;
            TextBox.SelectionIndent = indentLevel * IndentStep;
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
        private void CreateList(KeyEventArgs e)
        {
            TextBox.SelectionBullet = true;
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

    }
}