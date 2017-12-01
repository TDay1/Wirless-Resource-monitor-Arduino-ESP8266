using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Diagnostics;

namespace resourcemon
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// The backgroundworker object on which the time consuming operation shall be executed
        /// </summary>
        BackgroundWorker m_oWorker;
        bool cpubool;
        PerformanceCounter total_cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        string ipAdress = "192.168.0.75";
        int port = 7777;

        public Form1()
        {
            InitializeComponent();
            m_oWorker = new BackgroundWorker();
            m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            m_oWorker.ProgressChanged += new ProgressChangedEventHandler(m_oWorker_ProgressChanged);
            m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_oWorker_RunWorkerCompleted);
            m_oWorker.WorkerReportsProgress = true;
            m_oWorker.WorkerSupportsCancellation = true;
            cpubool = true;
            m_oWorker.RunWorkerAsync();
        }

        /// <summary>
        /// On completed do the appropriate task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
 
        }

        /// <summary>
        /// Notification is performed here to the progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        /// <summary>
        /// Time consuming operations go here </br>
        /// i.e. Database operations,Reporting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (cpubool == true)
            {
                float t = total_cpu.NextValue();
                string cpu = string.Format("{0:N2}", t);
                string cpu2 = String.Format("CPU: {0}%", cpu);
                System.Threading.Thread.Sleep(100);

                //Send UDP

                UdpClient udpClient = new UdpClient(ipAdress, port);
                Byte[] sendBytes = Encoding.ASCII.GetBytes(cpu2);
                try
                {
                    udpClient.Send(sendBytes, sendBytes.Length);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error. Could not send UDP packet. Trying again in 20 seconds", "Error");
                    Thread.Sleep(20000);
                }
            }
            //ytre
            string byeMessage = "Goodbye!  ";
            string clear = "";
            UdpClient udpClient2 = new UdpClient(ipAdress, port);
            Byte[] sendBytes2 = Encoding.ASCII.GetBytes(byeMessage);
            Byte[] sendBytes3 = Encoding.ASCII.GetBytes(clear);
            try
            {
                udpClient2.Send(sendBytes2, sendBytes2.Length);
                Thread.Sleep(750);
                udpClient2.Send(sendBytes3, sendBytes3.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Error. Could not send UDP packet. Trying again in 20 seconds", "Error");
                Thread.Sleep(20000);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (m_oWorker.IsBusy)
            {
                //Stop/Cancel the async operation here
                // m_oWorker.CancelAsync();
                cpubool = false;
            }
        }

        private void cPUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cPUToolStripMenuItem.Checked == true)
            {
                cpubool = true;
                //Start the async operation here
                m_oWorker.RunWorkerAsync();
            }
            else
            {
                if (m_oWorker.IsBusy)
                {
                    //Stop/Cancel the async operation here
                    // m_oWorker.CancelAsync();
                    cpubool = false;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
