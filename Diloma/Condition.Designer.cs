namespace Diloma
{
    partial class Condition
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.KeyRefreshTime = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.RefreshTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.UDPport = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TCPport = new System.Windows.Forms.TextBox();
            this.Layers = new System.Windows.Forms.TextBox();
            this.UDPmask = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TimeOut = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.MaxNode = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.MaxConnections = new System.Windows.Forms.TextBox();
            this.FakeMessages = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Save = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 191);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Последнее обновление ключа:";
            // 
            // KeyRefreshTime
            // 
            this.KeyRefreshTime.AutoSize = true;
            this.KeyRefreshTime.Location = new System.Drawing.Point(27, 222);
            this.KeyRefreshTime.Name = "KeyRefreshTime";
            this.KeyRefreshTime.Size = new System.Drawing.Size(62, 17);
            this.KeyRefreshTime.TabIndex = 4;
            this.KeyRefreshTime.Text = "datetime";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(179, 247);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(149, 48);
            this.button2.TabIndex = 5;
            this.button2.Text = "Обновить ключ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.TimeOut);
            this.groupBox2.Controls.Add(this.RefreshTime);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 274);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(328, 208);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Автоматическое обновление";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(224, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "секунд(ы)";
            // 
            // RefreshTime
            // 
            this.RefreshTime.Location = new System.Drawing.Point(150, 47);
            this.RefreshTime.Name = "RefreshTime";
            this.RefreshTime.Size = new System.Drawing.Size(68, 22);
            this.RefreshTime.TabIndex = 2;
            this.RefreshTime.Text = "300";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Обновлять каждые";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(128, 21);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(210, 46);
            this.button3.TabIndex = 7;
            this.button3.Text = "Открыть журнал анонимного сервера";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // UDPport
            // 
            this.UDPport.Location = new System.Drawing.Point(108, 33);
            this.UDPport.Name = "UDPport";
            this.UDPport.Size = new System.Drawing.Size(100, 22);
            this.UDPport.TabIndex = 8;
            this.UDPport.Text = "19999";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "UDP порт";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "TCP порт";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(174, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Степень перемешивания";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 17);
            this.label8.TabIndex = 12;
            this.label8.Text = "UDP маска";
            // 
            // TCPport
            // 
            this.TCPport.Location = new System.Drawing.Point(108, 93);
            this.TCPport.Name = "TCPport";
            this.TCPport.Size = new System.Drawing.Size(100, 22);
            this.TCPport.TabIndex = 14;
            this.TCPport.Text = "1456";
            // 
            // Layers
            // 
            this.Layers.Location = new System.Drawing.Point(222, 66);
            this.Layers.Name = "Layers";
            this.Layers.Size = new System.Drawing.Size(64, 22);
            this.Layers.TabIndex = 15;
            this.Layers.Text = "3";
            // 
            // UDPmask
            // 
            this.UDPmask.Location = new System.Drawing.Point(108, 152);
            this.UDPmask.Name = "UDPmask";
            this.UDPmask.Size = new System.Drawing.Size(188, 22);
            this.UDPmask.TabIndex = 16;
            this.UDPmask.Text = "192.168.88.255";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UDPport);
            this.groupBox1.Controls.Add(this.UDPmask);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.TCPport);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 223);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сетевые порты";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 159);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(145, 17);
            this.label9.TabIndex = 18;
            this.label9.Text = "Время поиска узлов:";
            // 
            // TimeOut
            // 
            this.TimeOut.Location = new System.Drawing.Point(150, 156);
            this.TimeOut.Name = "TimeOut";
            this.TimeOut.Size = new System.Drawing.Size(68, 22);
            this.TimeOut.TabIndex = 19;
            this.TimeOut.Text = "2";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(224, 159);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 17);
            this.label10.TabIndex = 4;
            this.label10.Text = "секунд(ы)";
            // 
            // MaxNode
            // 
            this.MaxNode.Location = new System.Drawing.Point(222, 31);
            this.MaxNode.Name = "MaxNode";
            this.MaxNode.Size = new System.Drawing.Size(64, 22);
            this.MaxNode.TabIndex = 18;
            this.MaxNode.Text = "10";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(27, 31);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(189, 17);
            this.label11.TabIndex = 19;
            this.label11.Text = "Максимальное число узлов";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 91);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(316, 48);
            this.label12.TabIndex = 20;
            this.label12.Text = "Отметка об автоматическом обновлении находится в окне списка узлов";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(27, 109);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(231, 17);
            this.label13.TabIndex = 20;
            this.label13.Text = "Максимальное число соединений";
            // 
            // MaxConnections
            // 
            this.MaxConnections.Location = new System.Drawing.Point(264, 106);
            this.MaxConnections.Name = "MaxConnections";
            this.MaxConnections.Size = new System.Drawing.Size(64, 22);
            this.MaxConnections.TabIndex = 21;
            this.MaxConnections.Text = "10";
            // 
            // FakeMessages
            // 
            this.FakeMessages.AutoSize = true;
            this.FakeMessages.Location = new System.Drawing.Point(30, 154);
            this.FakeMessages.Name = "FakeMessages";
            this.FakeMessages.Size = new System.Drawing.Size(216, 21);
            this.FakeMessages.TabIndex = 22;
            this.FakeMessages.Text = "Отправлять ложные пакеты";
            this.FakeMessages.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.FakeMessages);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.MaxConnections);
            this.groupBox3.Controls.Add(this.KeyRefreshTime);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.MaxNode);
            this.groupBox3.Controls.Add(this.Layers);
            this.groupBox3.Location = new System.Drawing.Point(356, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(353, 307);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Дополнительные параметры";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(128, 84);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(210, 46);
            this.button1.TabIndex = 24;
            this.button1.Text = "Открыть журнал локального сервера";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Location = new System.Drawing.Point(356, 346);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(353, 136);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Журналирование";
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(559, 511);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(150, 48);
            this.Save.TabIndex = 26;
            this.Save.Text = "Применить";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(386, 511);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(150, 48);
            this.Cancel.TabIndex = 27;
            this.Cancel.Text = "Отмена";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Condition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 577);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Condition";
            this.Text = "Администрирование";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label KeyRefreshTime;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox RefreshTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox UDPport;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TCPport;
        private System.Windows.Forms.TextBox Layers;
        private System.Windows.Forms.TextBox UDPmask;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TimeOut;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox MaxNode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox MaxConnections;
        private System.Windows.Forms.CheckBox FakeMessages;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Cancel;
    }
}