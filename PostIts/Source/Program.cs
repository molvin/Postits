using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace PostIts
{
    static class Program
    {
        private static int IdCounter = 0;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            List<SaveManager.SaveData> postits = SaveManager.LoadFiles();
            if(postits == null)
            {
                PostItForm form = new PostItForm(0, "", true, null, null);
                Application.Run(form);
                return;
            }

            IdCounter = postits.Count;
            bool select = true;
            foreach (SaveManager.SaveData save in postits)
            {
                NewWindow(save.Id, save.RichText, select, new Point(save.X, save.Y), new Size(save.Width, save.Height));
                select = false;
            }
        }
        public static void NewWindow()
        {
            NewWindow(IdCounter++, "", true, null, null);
        }
        public static void NewWindow(int id, string rtf, bool select, Point? point, Size? size)
        {
            Thread thread = new Thread(() => Application.Run(new PostItForm(id, rtf, select, point, size)));
            thread.Start();
        }
    }
}
