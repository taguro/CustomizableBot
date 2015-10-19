using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CustomizableBot
{
    public partial class ControlForm : Form
    {
        Bot bot;

        public ControlForm()
        {
            InitializeComponent();
            bot = new Bot(this);
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            button_start.Enabled = false;
            button_stop.Enabled = true;
            button_restart.Enabled = true;
            listBox_log.Items.Add("start");
            bot.start();
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            listBox_log.Items.Add("stop");
            bot.stop();
            button_start.Enabled = true;
            button_stop.Enabled = false;
        }

        private void button_restart_Click(object sender, EventArgs e)
        {
            button_start.Enabled = false;
            button_stop.Enabled = true;
            listBox_log.Items.Add("restart");
            bot.restart();
        }

        delegate void del();
        internal void log(string str)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBox_log.InvokeRequired)
            {
                Action act = new Action(() => { this.listBox_log.Items.Add(str); });
                this.Invoke(act , new object[] {  });
            }
            else
            {
                this.listBox_log.Items.Add(str);
            }
        }
        internal void logWithDispose(Bitmap bmp)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.pictureBox_screen.InvokeRequired)
            {
                Action act = new Action(() => {
                    this.pictureBox_screen.Image = bmp;
                    this.Refresh();
                    bmp.Dispose();
                });
                this.Invoke(act, new object[] { });
            }
            else
            {
                this.pictureBox_screen.Image = bmp;
                this.Refresh();
                bmp.Dispose();
            }
        }

        #region
        //Botに入力する画面の範囲をpicutureBoxでのドラッグアンドドロップで指定する
        MouseEventArgs start;
        private void pictureBox_screen_MouseDown(object sender, MouseEventArgs e)
        {
            start = e;
        }
        MouseEventArgs end;
        private void pictureBox_screen_MouseUp(object sender, MouseEventArgs e)
        {
            end = e;

            if (start.X == end.X || start.Y == end.Y) return;//ドラッグして範囲指定していない場合は何も実行しない
            using (Bitmap src = UIControl.captureScreen())
            {
                pictureBox_screen.Height = pictureBox_screen.Height;
                pictureBox_screen.Width = pictureBox_screen.Height * src.Width / src.Height;

                int left = src.Width * start.X / pictureBox_screen.Width;
                int up = src.Height * start.Y / pictureBox_screen.Height;
                int right = src.Width * end.X  / pictureBox_screen.Width;
                int down = src.Height * end.Y / pictureBox_screen.Height;

                if (right >= src.Width) right = src.Width - 1;
                if (down >= src.Height) down = src.Height - 1;
                if (right < 0) right = 0;
                if (down < 0) down = 0;

                if (start.X > end.X)
                {
                    int temp = left;
                    left = right;
                    right = temp;
                }
                if (start.Y > end.Y)
                {
                    int temp = up;
                    up = down;
                    down = temp;
                }

                bot.setTrimRange(left, up, right, down);
                log("select left=" + left + ", up=" + up + ", right=" + right + ", down=" + down);
            }
        }
        #endregion
        private void pictureBox_screen_Click(object sender, MouseEventArgs e)
        {
            if (start.X != e.X && start.Y != e.Y) return;//ドラッグして範囲指定している場合は何も実行しない
            using (Bitmap src = UIControl.captureScreen())
            {
                pictureBox_screen.Height = pictureBox_screen.Height;
                pictureBox_screen.Width = pictureBox_screen.Height * src.Width / src.Height;

                int x = src.Width * e.X / pictureBox_screen.Width;
                int y = src.Height * e.Y / pictureBox_screen.Height;
                bot.addInputPoints(x,y);
                log("click x:"+x+", y:"+y);
            }
        }

        private void button_save_bot_Click(object sender, EventArgs e)
        {
            bot.save();
        }

        private void button_load_bot_Click(object sender, EventArgs e)
        {
            bot.load();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            bot.deleteInputPoints();
        }
    }
}
