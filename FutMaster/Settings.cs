using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FutMaster
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            metroSetTextBox1.Text = Properties.Settings.Default.email;
            metroSetTextBox2.Text = Properties.Settings.Default.password;
            checkBox1.Checked =Properties.Settings.Default.autofill;
            numericUpDown1.Value = Properties.Settings.Default.minDelay;
            numericUpDown2.Value = Properties.Settings.Default.maxDelay;
        }

        private void Settings_FormClosing(Object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.email = metroSetTextBox1.Text;
            Properties.Settings.Default.password = metroSetTextBox2.Text;
            Properties.Settings.Default.autofill = checkBox1.Checked;
            Properties.Settings.Default.minDelay = decimal.ToInt32(numericUpDown1.Value);
            Properties.Settings.Default.maxDelay = decimal.ToInt32(numericUpDown2.Value);
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
        }
    }
}
