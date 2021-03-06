﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace YATA {
    public partial class Prefs : Form {

        #region strings
        List<String> messages = new List<string>() {
            "If you don't update YATA+, you may miss some important new features in the next updates....",
        "You may need YATA+ to fully apply the settings",
        "Using a sample rate not included in the list may corrupt the sound"};
        #endregion
        bool NoSettings = false;
        public Prefs(bool _NoSettings = false) {
            InitializeComponent();
            NoSettings = _NoSettings;
            comboBox1.Items.Add("english");
            if (Directory.Exists("languages"))
            {
                foreach (string dir in Directory.GetDirectories("languages"))
                {
                    comboBox1.Items.Add(dir.Remove(0, 10));
                }
            }
            chb_opt.Checked = Form1.APP_not_Optimize_Cwavs;
            chb_UISim.Checked = Form1.APP_ShowUI_Sim;
            chb_SavePrev.Checked = Form1.APP_AutoGen_preview;
            chb_wait.Checked = Form1.APP_Wait_editor;
            chb_delTempFile.Checked = Form1.APP_Clean_On_exit;
            chb_loadBGM.Checked = Form1.APP_Auto_Load_bgm;
            chb_updates.Checked = Form1.APP_check_UPD;
            chb_ExportBot.Checked = Form1.APP_export_both_screens;
            chb_extplayer.Checked = Form1.APP_use_ext_player;
            Cmb_OPT.Text = Form1.APP_opt_samples.ToString();
            textBox1.Text = Form1.APP_photo_edtor;
            numericUpDown1.Value = Form1.APP_Move_buttons_colors;
            numericUpDown2.Value = Form1.APP_SETT_SIZE_X;
            numericUpDown3.Value = Form1.APP_SETT_SIZE_Y;
            comboBox1.Text = Form1.APP_LNG;
            try { 
            #region language
            if (Form1.APP_LNG.Trim().ToLower() != "english" && File.Exists(@"languages\" + Form1.APP_LNG + @"\prefs.txt"))
            {
                messages.Clear();
                string[] lng = File.ReadAllLines(@"languages\" + Form1.APP_LNG + @"\prefs.txt");
                foreach (string line in lng)
                {
                    if (!line.StartsWith(";"))
                    {                        
                        string[] tmp = line.Replace(@"\r\n", Environment.NewLine).Split(Convert.ToChar("="));
                        if (line.StartsWith("btn")) { ((Button)this.Controls.Find(tmp[0], true)[0]).Text = tmp[1]; }
                        else if (line.StartsWith("lbl")) { ((Label)this.Controls.Find(tmp[0], true)[0]).Text = tmp[1]; }
                        else if (line.StartsWith("chb")) { ((CheckBox)this.Controls.Find(tmp[0], true)[0]).Text = tmp[1]; }
                        else if (line.StartsWith("@")) { messages.Add(line.Replace(@"\r\n", Environment.NewLine).Remove(0, 1)); }
                    }
                }
            }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error initializing the language data for this window, try to set the language to english, if you can't because the settings windows crashes too, delete the languages folder");
                MessageBox.Show("for translators: 'Lbl_something' is diffrent from 'lbl_something', follow the template");
                MessageBox.Show("Exception details: " + ex.Message);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chb_UISim.Checked = true;
            chb_SavePrev.Checked = false;
            chb_wait.Checked = true;
            chb_delTempFile.Checked = false;
            chb_loadBGM.Checked = true;
            chb_updates.Checked = true;
            chb_ExportBot.Checked = true;
            chb_opt.Checked = false;
            chb_extplayer.Checked = false;
            Cmb_OPT.Text = "11025";
            comboBox1.Text = "english";
            numericUpDown1.Value = 10;
            Form1.APP_photo_edtor = "";
            build_settings();
            Form1.load_prefs();
            this.Close();
        }

        public void build_settings()
        {
            string[] lines = new string[16];
            lines[0] = "ui_sim=" + chb_UISim.Checked.ToString();
            lines[1] = "gen_prev=" + chb_SavePrev.Checked.ToString();
            lines[2] = "photo_edit=" + textBox1.Text;
            lines[3] = "wait_editor=" + chb_wait.Checked.ToString();
            lines[4] = "clean_on_exit=" + chb_delTempFile.Checked.ToString();
            lines[5] = "load_bgm=" + chb_loadBGM.Checked.ToString();
            lines[6] = "first_start_v7=" + NoSettings.ToString();
            lines[7] = "shift_btns=" + numericUpDown1.Value.ToString();
            lines[8] = "check_updates=" + chb_updates.Checked.ToString();
            lines[9] = "happy_easter=false";
            lines[10] = "sett_size=" + numericUpDown2.Value.ToString() + numericUpDown3.Value.ToString();
            lines[11] = "exp_both_screens=" + chb_ExportBot.Checked.ToString();
            lines[12] = "lng=" + comboBox1.Text;
            lines[13] = "n_opt_cwavs=" + chb_opt.Checked;
            if (!Cmb_OPT.Items.Contains(Cmb_OPT.Text)) MessageBox.Show(messages[2], "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            lines[14] = "opt_samples=" + Convert.ToInt32(Cmb_OPT.Text).ToString();
            lines[15] = "ext_player=" + chb_extplayer.Checked;
            System.IO.File.Delete("Settings.ini");
            System.IO.File.WriteAllLines("Settings.ini", lines);
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            build_settings();
            Form1.load_prefs();
            MessageBox.Show(messages[1]);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (!chb_updates.Checked) MessageBox.Show(messages[0]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=-_c-JKx1Lvg");
        }

        private void Prefs_Load(object sender, EventArgs e)
        {

        }
    }
}
