namespace football_automatic
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox_searchQuery = new TextBox();
            comboBox_searchEntity = new ComboBox();
            button_search = new Button();
            listBox1 = new ListBox();
            button1 = new Button();
            label_selectedMatch = new Label();
            button_gamesToday = new Button();
            comboBox_country = new ComboBox();
            comboBox_league = new ComboBox();
            label_clock = new Label();
            button_startClock = new Button();
            tabControl1 = new TabControl();
            tabPage_search = new TabPage();
            button_setClock = new Button();
            textBox_setSeconds = new TextBox();
            textBox_setMinutes = new TextBox();
            button_pauseClock = new Button();
            button_resetClock = new Button();
            tabPage2 = new TabPage();
            tabControl1.SuspendLayout();
            tabPage_search.SuspendLayout();
            SuspendLayout();
            // 
            // textBox_searchQuery
            // 
            textBox_searchQuery.Location = new Point(12, 12);
            textBox_searchQuery.Name = "textBox_searchQuery";
            textBox_searchQuery.Size = new Size(190, 23);
            textBox_searchQuery.TabIndex = 0;
            // 
            // comboBox_searchEntity
            // 
            comboBox_searchEntity.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_searchEntity.FormattingEnabled = true;
            comboBox_searchEntity.Location = new Point(208, 12);
            comboBox_searchEntity.Name = "comboBox_searchEntity";
            comboBox_searchEntity.Size = new Size(121, 23);
            comboBox_searchEntity.TabIndex = 1;
            comboBox_searchEntity.SelectedIndexChanged += comboBox_searchEntity_SelectedIndexChanged;
            // 
            // button_search
            // 
            button_search.Location = new Point(208, 41);
            button_search.Name = "button_search";
            button_search.Size = new Size(121, 23);
            button_search.TabIndex = 2;
            button_search.Text = "Search";
            button_search.UseVisualStyleBackColor = true;
            button_search.Click += button_search_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 139);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(591, 214);
            listBox1.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(12, 359);
            button1.Name = "button1";
            button1.Size = new Size(141, 23);
            button1.TabIndex = 4;
            button1.Text = "Select Match";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button_selectMatch;
            // 
            // label_selectedMatch
            // 
            label_selectedMatch.AutoSize = true;
            label_selectedMatch.Location = new Point(15, 395);
            label_selectedMatch.Name = "label_selectedMatch";
            label_selectedMatch.Size = new Size(38, 15);
            label_selectedMatch.TabIndex = 5;
            label_selectedMatch.Text = "label1";
            // 
            // button_gamesToday
            // 
            button_gamesToday.Location = new Point(12, 41);
            button_gamesToday.Name = "button_gamesToday";
            button_gamesToday.Size = new Size(129, 23);
            button_gamesToday.TabIndex = 6;
            button_gamesToday.Text = "Games Today";
            button_gamesToday.UseVisualStyleBackColor = true;
            button_gamesToday.Click += button_gamesToday_Click;
            // 
            // comboBox_country
            // 
            comboBox_country.FormattingEnabled = true;
            comboBox_country.Location = new Point(142, 93);
            comboBox_country.Name = "comboBox_country";
            comboBox_country.Size = new Size(121, 23);
            comboBox_country.TabIndex = 7;
            comboBox_country.Text = "Country";
            comboBox_country.SelectedIndexChanged += comboBox_country_SelectedIndexChanged;
            // 
            // comboBox_league
            // 
            comboBox_league.FormattingEnabled = true;
            comboBox_league.Location = new Point(298, 93);
            comboBox_league.Name = "comboBox_league";
            comboBox_league.Size = new Size(121, 23);
            comboBox_league.TabIndex = 8;
            comboBox_league.Text = "League";
            comboBox_league.SelectedIndexChanged += comboBox_league_SelectedIndexChanged;
            // 
            // label_clock
            // 
            label_clock.AutoSize = true;
            label_clock.Location = new Point(337, 384);
            label_clock.Name = "label_clock";
            label_clock.Size = new Size(37, 15);
            label_clock.TabIndex = 9;
            label_clock.Text = "Clock";
            label_clock.Click += label_clock_Click;
            // 
            // button_startClock
            // 
            button_startClock.Location = new Point(410, 380);
            button_startClock.Name = "button_startClock";
            button_startClock.Size = new Size(75, 23);
            button_startClock.TabIndex = 10;
            button_startClock.Text = "Start";
            button_startClock.UseVisualStyleBackColor = true;
            button_startClock.Click += button_startClock_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage_search);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.HotTrack = true;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 12;
            // 
            // tabPage_search
            // 
            tabPage_search.AutoScroll = true;
            tabPage_search.Controls.Add(button_setClock);
            tabPage_search.Controls.Add(textBox_setSeconds);
            tabPage_search.Controls.Add(textBox_setMinutes);
            tabPage_search.Controls.Add(button_pauseClock);
            tabPage_search.Controls.Add(button_resetClock);
            tabPage_search.Controls.Add(comboBox_searchEntity);
            tabPage_search.Controls.Add(button_startClock);
            tabPage_search.Controls.Add(listBox1);
            tabPage_search.Controls.Add(textBox_searchQuery);
            tabPage_search.Controls.Add(label_selectedMatch);
            tabPage_search.Controls.Add(label_clock);
            tabPage_search.Controls.Add(button_gamesToday);
            tabPage_search.Controls.Add(button_search);
            tabPage_search.Controls.Add(button1);
            tabPage_search.Controls.Add(comboBox_league);
            tabPage_search.Controls.Add(comboBox_country);
            tabPage_search.Location = new Point(4, 24);
            tabPage_search.Name = "tabPage_search";
            tabPage_search.Padding = new Padding(3);
            tabPage_search.Size = new Size(792, 422);
            tabPage_search.TabIndex = 0;
            tabPage_search.Text = "Search";
            tabPage_search.UseVisualStyleBackColor = true;
            // 
            // button_setClock
            // 
            button_setClock.Location = new Point(674, 380);
            button_setClock.Name = "button_setClock";
            button_setClock.Size = new Size(75, 23);
            button_setClock.TabIndex = 15;
            button_setClock.Text = "Set Clock";
            button_setClock.UseVisualStyleBackColor = true;
            button_setClock.Click += button_setClock_Click;
            // 
            // textBox_setSeconds
            // 
            textBox_setSeconds.Location = new Point(669, 339);
            textBox_setSeconds.Name = "textBox_setSeconds";
            textBox_setSeconds.Size = new Size(100, 23);
            textBox_setSeconds.TabIndex = 14;
            // 
            // textBox_setMinutes
            // 
            textBox_setMinutes.Location = new Point(669, 310);
            textBox_setMinutes.Name = "textBox_setMinutes";
            textBox_setMinutes.Size = new Size(100, 23);
            textBox_setMinutes.TabIndex = 13;
            // 
            // button_pauseClock
            // 
            button_pauseClock.Location = new Point(491, 380);
            button_pauseClock.Name = "button_pauseClock";
            button_pauseClock.Size = new Size(75, 23);
            button_pauseClock.TabIndex = 12;
            button_pauseClock.Text = "Stop";
            button_pauseClock.UseVisualStyleBackColor = true;
            button_pauseClock.Click += button_pauseClock_Click;
            // 
            // button_resetClock
            // 
            button_resetClock.Location = new Point(572, 380);
            button_resetClock.Name = "button_resetClock";
            button_resetClock.Size = new Size(75, 23);
            button_resetClock.TabIndex = 11;
            button_resetClock.Text = "Reset";
            button_resetClock.UseVisualStyleBackColor = true;
            button_resetClock.Click += button_resetClock_Click;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 422);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabPage_search.ResumeLayout(false);
            tabPage_search.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox textBox_searchQuery;
        private ComboBox comboBox_searchEntity;
        private Button button_search;
        private ListBox listBox1;
        private Button button1;
        private Label label_selectedMatch;
        private Button button_gamesToday;
        private ComboBox comboBox_country;
        private ComboBox comboBox_league;
        private Label label_clock;
        private Button button_startClock;
        private TabControl tabControl1;
        private TabPage tabPage2;
        private TabPage tabPage_search;
        private Button button_resetClock;
        private Button button_pauseClock;
        private Button button_setClock;
        private TextBox textBox_setSeconds;
        private TextBox textBox_setMinutes;
    }
}