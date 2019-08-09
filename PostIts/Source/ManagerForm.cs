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
    public partial class ManagerForm : Form
    {
        private static ManagerForm instance;
        private static SynchronizationContext contextInstance;
        private static int IdCounter = 0;
        private static readonly List<(SynchronizationContext, PostItForm)> contextForms = new List<(SynchronizationContext, PostItForm)>();
        private static readonly HashSet<int> openForms = new HashSet<int>();

        public ManagerForm()
        {
            instance = this;
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = true;

            List<SaveManager.SaveData> postits = SaveManager.LoadFiles();
            if (postits == null)
            {
                NewWindow();
                return;
            }

            IdCounter = postits.Count;
            bool select = true;
            foreach (SaveManager.SaveData save in postits)
            {
                NewWindow(save.Id, save.RichText, select, new Point(save.X, save.Y), new Size(save.Width, save.Height));
                select = false;
            }

            Activated += (x, y) => OnFocus();
            contextInstance = SynchronizationContext.Current;
        }
        private void ManagerForm_Load(object sender, EventArgs e)
        {
            Size = new Size(0, 0);
        }
        public static void NewWindow()
        {
            NewWindow(IdCounter++, "", true, null, null);
        }
        public static void NewWindow(int id, string rtf, bool select, Point? point, Size? size)
        {
            openForms.Add(id);
            Thread thread = new Thread(() => Application.Run(new PostItForm(id, rtf, select, point, size)));
            thread.Start();
        }
        public static void AddContextForm(SynchronizationContext context, PostItForm form)
        {
            contextForms.Add((context, form));
        }
        public static void OnCloseForm(int formId)
        {
            openForms.Remove(formId);
            if (openForms.Count == 0)
                contextInstance.Post(new SendOrPostCallback(s => instance.Close()), null);               
        }
        private void OnFocus()
        {
            foreach((SynchronizationContext context, PostItForm form) in contextForms)
            {
                context.Post(new SendOrPostCallback(Sync), form);
            }
            void Sync(object state)
            {
                (state as PostItForm).Activate();
            }
        }
    }
}
