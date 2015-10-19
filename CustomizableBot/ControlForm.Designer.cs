namespace CustomizableBot
{
    partial class ControlForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_start = new System.Windows.Forms.Button();
            this.button_stop = new System.Windows.Forms.Button();
            this.listBox_log = new System.Windows.Forms.ListBox();
            this.button_restart = new System.Windows.Forms.Button();
            this.pictureBox_screen = new System.Windows.Forms.PictureBox();
            this.button_save_bot = new System.Windows.Forms.Button();
            this.button_load_bot = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.textBox_granularity = new System.Windows.Forms.TextBox();
            this.label_granularity = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_screen)).BeginInit();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(312, 12);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 0;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_stop
            // 
            this.button_stop.Enabled = false;
            this.button_stop.Location = new System.Drawing.Point(312, 41);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(75, 23);
            this.button_stop.TabIndex = 1;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // listBox_log
            // 
            this.listBox_log.FormattingEnabled = true;
            this.listBox_log.ItemHeight = 12;
            this.listBox_log.Location = new System.Drawing.Point(12, 12);
            this.listBox_log.Name = "listBox_log";
            this.listBox_log.Size = new System.Drawing.Size(266, 148);
            this.listBox_log.TabIndex = 2;
            // 
            // button_restart
            // 
            this.button_restart.Enabled = false;
            this.button_restart.Location = new System.Drawing.Point(312, 70);
            this.button_restart.Name = "button_restart";
            this.button_restart.Size = new System.Drawing.Size(75, 23);
            this.button_restart.TabIndex = 3;
            this.button_restart.Text = "Restart";
            this.button_restart.UseVisualStyleBackColor = true;
            this.button_restart.Click += new System.EventHandler(this.button_restart_Click);
            // 
            // pictureBox_screen
            // 
            this.pictureBox_screen.BackColor = System.Drawing.SystemColors.Desktop;
            this.pictureBox_screen.Location = new System.Drawing.Point(13, 166);
            this.pictureBox_screen.Name = "pictureBox_screen";
            this.pictureBox_screen.Size = new System.Drawing.Size(265, 172);
            this.pictureBox_screen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_screen.TabIndex = 4;
            this.pictureBox_screen.TabStop = false;
            this.pictureBox_screen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_screen_Click);
            this.pictureBox_screen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_screen_MouseDown);
            this.pictureBox_screen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_screen_MouseUp);
            // 
            // button_save_bot
            // 
            this.button_save_bot.Location = new System.Drawing.Point(312, 99);
            this.button_save_bot.Name = "button_save_bot";
            this.button_save_bot.Size = new System.Drawing.Size(94, 23);
            this.button_save_bot.TabIndex = 7;
            this.button_save_bot.Text = "Botを保存";
            this.button_save_bot.UseVisualStyleBackColor = true;
            this.button_save_bot.Click += new System.EventHandler(this.button_save_bot_Click);
            // 
            // button_load_bot
            // 
            this.button_load_bot.Location = new System.Drawing.Point(312, 128);
            this.button_load_bot.Name = "button_load_bot";
            this.button_load_bot.Size = new System.Drawing.Size(94, 23);
            this.button_load_bot.TabIndex = 8;
            this.button_load_bot.Text = "Botを読込";
            this.button_load_bot.UseVisualStyleBackColor = true;
            this.button_load_bot.Click += new System.EventHandler(this.button_load_bot_Click);
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(312, 157);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(94, 23);
            this.button_delete.TabIndex = 9;
            this.button_delete.Text = "入力点を消去";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // textBox_granularity
            // 
            this.textBox_granularity.Location = new System.Drawing.Point(377, 202);
            this.textBox_granularity.Name = "textBox_granularity";
            this.textBox_granularity.Size = new System.Drawing.Size(29, 19);
            this.textBox_granularity.TabIndex = 10;
            this.textBox_granularity.Text = "10";
            this.textBox_granularity.TextChanged += new System.EventHandler(this.textBox_granularity_TextChanged);
            // 
            // label_granularity
            // 
            this.label_granularity.AutoSize = true;
            this.label_granularity.Location = new System.Drawing.Point(321, 187);
            this.label_granularity.Name = "label_granularity";
            this.label_granularity.Size = new System.Drawing.Size(75, 12);
            this.label_granularity.TabIndex = 11;
            this.label_granularity.Text = "入力点の間隔";
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 350);
            this.Controls.Add(this.label_granularity);
            this.Controls.Add(this.textBox_granularity);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_load_bot);
            this.Controls.Add(this.button_save_bot);
            this.Controls.Add(this.pictureBox_screen);
            this.Controls.Add(this.button_restart);
            this.Controls.Add(this.listBox_log);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.button_start);
            this.Name = "ControlForm";
            this.Text = "ControlForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_screen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.ListBox listBox_log;
        private System.Windows.Forms.Button button_restart;
        private System.Windows.Forms.PictureBox pictureBox_screen;
        private System.Windows.Forms.Button button_save_bot;
        private System.Windows.Forms.Button button_load_bot;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.TextBox textBox_granularity;
        private System.Windows.Forms.Label label_granularity;
    }
}

