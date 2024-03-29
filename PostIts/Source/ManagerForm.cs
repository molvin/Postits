﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
        //private static readonly List<(SynchronizationContext, PostItForm)> contextForms = new List<(SynchronizationContext, PostItForm)>();
        //private static readonly HashSet<int> openForms = new HashSet<int>();
        private static readonly Dictionary<int, SynchronizationContext> contextForms = new Dictionary<int, SynchronizationContext>();
        private static readonly Dictionary<int, PostItForm> openForms = new Dictionary<int, PostItForm>();


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
                if (!save.Open) continue;
                NewWindow(save.Id, save.RichText, select, new Point(save.X, save.Y), new Size(save.Width, save.Height));
                select = false;
            }
            if (select)
                SavesForm.OpenSavesForm();

            contextInstance = SynchronizationContext.Current;
            FormClosed += (x, y) => PostToForms(form => form.Close());
            
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Console.WriteLine("Lost focus");
            base.OnLostFocus(e);
        }
        protected override void OnGotFocus(EventArgs e)
        {
            Console.WriteLine("Got focus");
            PostToForms(form => form.Activate());
            base.OnGotFocus(e);
        }

        private void ManagerForm_Load(object sender, EventArgs e)
        {
            Size = new Size(100, 100);
            Opacity = 0.0f;
        }
        public static void NewWindow()
        {
            NewWindow(IdCounter++, "", true, null, null);
        }
        public static void NewWindow(int id, string rtf, bool select, Point? point, Size? size)
        {
            if (openForms.ContainsKey(id)) return;
            Thread thread = new Thread(() => Application.Run(new PostItForm(id, rtf, select, point, size)));

            thread.Start();
        }
        public static void AddContextForm(SynchronizationContext context, PostItForm form)
        {
            lock(openForms)
            {
                openForms.Add(form.Id, form);
            }
            lock(contextForms)
            {
                contextForms.Add(form.Id, context);
            }
        }
        public static bool OnCloseForm(int formId)
        {
            openForms.Remove(formId);
            contextForms.Remove(formId);
            if (openForms.Count > 0)
                return false;
            contextInstance.Post(new SendOrPostCallback(s => instance.Close()), null);
            return true;
        }
        public static void OnDeleteForm(int formId)
        {
            if (!openForms.ContainsKey(formId))
                return;


            lock(contextForms)
            {
                contextForms[formId].Post(new SendOrPostCallback(state => (state as PostItForm).Exit(false)), openForms[formId]);
            }
        }

        private void ShowForms(bool show)
        {
            if (show)
            {
                PostToForms(form => form.Show());
                PostToForms(form => form.Activate());
                PostToForms(form => form.BringToFront());

            }
            else
                PostToForms(form => form.Hide());
        }
        private void PostToForms(Action<PostItForm> action)
        {
            lock(contextForms)
            {
                foreach (var pair in contextForms)
                {
                    if(openForms.ContainsKey(pair.Key))
                        pair.Value.Post(new SendOrPostCallback(Sync), openForms[pair.Key]);
                }
            }
     
            void Sync(object state)
            {
                action?.Invoke(state as PostItForm);
            }
        }
    }
}
