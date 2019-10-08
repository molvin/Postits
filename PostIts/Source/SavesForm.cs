using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace PostIts
{
    public partial class SavesForm : Form
    {
        public static SavesForm Instance;
        private int selectedId = -1;
        private Dictionary<int, Button> buttons = new Dictionary<int, Button>();

        public SavesForm()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                throw new Exception("Only one instance allowed");
            }

            InitializeComponent();
        }
        public static void OpenSavesForm()
        {
            if (Instance != null) return;
            Thread thread = new Thread(() => Application.Run(new SavesForm()));
            thread.Start();
        }

        private void SavesForm_Load(object sender, EventArgs e)
        {
            OpenButton.Click += (x, y) => OpenForm();
            DeleteButton.Click += (x, y) => DeleteForm();
            FormClosed += (x, y) => Instance = null;

            List<SaveManager.SaveData> postits = SaveManager.LoadFiles();

            foreach(SaveManager.SaveData save in postits)
            {
                Button button = new Button();
                buttons.Add(save.Id, button);
                button.Text = save.Id.ToString();
                button.Click += (x,y) => selectedId = save.Id;
                flowLayoutPanel1.Controls.Add(button);
            }
        }

        private void OpenForm()
        {
            if (selectedId == -1) return;

            List<SaveManager.SaveData> postits = SaveManager.LoadFiles();
            SaveManager.SaveData save = postits[selectedId];
            ManagerForm.NewWindow(save.Id, save.RichText, true, new Point(save.X, save.Y), new Size(save.Width, save.Height));
        }
        private void DeleteForm()
        {
            if (selectedId == -1) return;

            flowLayoutPanel1.Controls.Remove(buttons[selectedId]);
            buttons.Remove(selectedId);
            ManagerForm.OnDeleteForm(selectedId);
            SaveManager.Delete(selectedId);
        }
    }


}

