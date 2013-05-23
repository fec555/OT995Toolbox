using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using OT995.Models;

namespace OT995
{
    public partial class Form1 : Form
    {
        RomRepository _romRepository = new RomRepository();
        List<RomFile> _roms = new List<RomFile>();
        private int _selectedIndex;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateListFromCsv();
        }

        private void CreateListFromCsv()
        {
            Stream readThis = _romRepository.GetRomList("");
            StreamReader reader = new StreamReader(readThis);
            
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                _roms.Add(new RomFile(){name = values[0], url = values[1], description = values[2]});
            }

            foreach(var rom in _roms)
            {
                listBox1.Items.Add(rom.name);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();

            _selectedIndex = listBox1.SelectedIndex;

            textBox1.Text = _roms[_selectedIndex].description;
        }

        private void btn_Download_Click(object sender, EventArgs e)
        {
            Thread bgThread = new Thread(() =>
                                             {
                                                 WebClient client = new WebClient();
                                                 client.DownloadProgressChanged +=
                                                     new DownloadProgressChangedEventHandler(
                                                         client_DownloadProgressChanged);
                                                 client.DownloadFileCompleted +=
                                                     new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                                                 client.DownloadFileAsync(
                                                     new Uri(_roms[_selectedIndex].url),
                                                     @"C:\testfile.zip");
                                             });
            bgThread.Start();
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                lblDownload.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive;
                progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                label2.Text = "Completed";
            });
        }
    }
}