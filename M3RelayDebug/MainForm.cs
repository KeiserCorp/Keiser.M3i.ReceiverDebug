using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace M3RelayDebug
{
    public partial class MainForm : Form
    {
        private Receiver receiver;
        private Logger logger;

        public MainForm()
        {
            InitializeComponent();
            logger = new Logger(this.outputBox);
            receiver = new Receiver(logger);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.setRunState();
        }

        private void setErrorMessage(string message)
        {
            this.errorStatus.ForeColor = System.Drawing.Color.Red;
            this.errorStatus.Text = message;
        }

        private void clearErrorMessage()
        {
            this.errorStatus.Text = "";
        }

        private void ipAddressBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string errorMessage = "Invalid IPv4 Address";

            if (ipAddress_isValid(tb.Text))
            {
                tb.Tag = true;
                tb.BackColor = System.Drawing.SystemColors.Window;
                if (this.errorStatus.Text == errorMessage)
                    clearErrorMessage();
            }
            else
            {
                tb.Tag = false;
                tb.BackColor = Color.Red;
                setErrorMessage(errorMessage);
            }
        }

        private void portBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string errorMessage = "Invalid Port (Range 1 - 65535)";

            if (portBox_isValid(tb.Text))
            {
                tb.Tag = true;
                tb.BackColor = System.Drawing.SystemColors.Window;
                if (this.errorStatus.Text == errorMessage)
                    clearErrorMessage();
            }
            else
            {
                tb.Tag = false;
                tb.BackColor = Color.Red;
                setErrorMessage(errorMessage);
            }
        }

        private bool portBox_isValid(string portString)
        {
            int port = Convert.ToInt32(portString);
            return (port > 0 && port <= 65535);
        }

        private static bool ipAddress_isValid(string ipAddress)
        {
            IPAddress unused;
            return IPAddress.TryParse(ipAddress, out unused)
              &&
              (
                  unused.AddressFamily != AddressFamily.InterNetwork
                  ||
                  ipAddress.Count(c => c == '.') == 3
              );
        }

        private bool isFormValid()
        {
            if (ipAddress_isValid(this.ipAddressBox.Text) && portBox_isValid(this.portBox.Text))
                return true;
            ipAddressBox_Validating(this.ipAddressBox, null);
            portBox_Validating(this.portBox, null);
            return false;
        }

        private void ipAddressBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !(e.KeyChar == '.') && !(e.KeyChar == (char)Keys.Back) && !(e.KeyChar == (char)Keys.Left) && !(e.KeyChar == (char)Keys.Right))
            {
                e.Handled = true;
            }
        }

        private void portBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !(e.KeyChar == (char)Keys.Back) && !(e.KeyChar == (char)Keys.Left) && !(e.KeyChar == (char)Keys.Right))
            {
                e.Handled = true;
            }
        }

        private void threadToggleButton_Click(object sender, EventArgs e)
        {
            if ((!receiver.running && isFormValid()) || receiver.running)
            {
                if (!receiver.running)
                {
                    DisableControls(this);
                    setRecieverAtts();
                    receiver.start();
                }
                else
                {
                    receiver.stop();
                    EnableControls(this);
                }
                setRunState();
            }
        }

        private void setRecieverAtts()
        {
            receiver.ipAddress = this.ipAddressBox.Text;
            receiver.ipPort = Convert.ToUInt16(this.portBox.Text);
        }

        private void setRunState()
        {
            if (receiver.running)
            {
                this.threadToggleButton.Text = "Stop";
                this.statusBarCurrentState.BackColor = System.Drawing.Color.Green;
                this.statusBarCurrentState.Text = "Running";
            }
            else
            {
                this.threadToggleButton.Text = "Start";
                this.statusBarCurrentState.BackColor = System.Drawing.Color.Red;
                this.statusBarCurrentState.Text = "Stopped";
            }
        }

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                DisableControls(c);
            }
            if (con is CheckBox || con is TextBox)
                con.Enabled = false;
        }

        private void EnableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                EnableControls(c);
            }
            if (con is CheckBox || con is TextBox)
                con.Enabled = true;
        }

        private void statusBarSaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "txt files (*.txt)|*.txt|log files (*.log)|*.log";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    sw.Write(logger.getLog());
            }
        }

        private void statusBarClearButton_Click(object sender, EventArgs e)
        {
            logger.reset();
        }
    }
}
