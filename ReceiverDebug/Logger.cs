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

namespace Keiser.M3i.ReceiverDebug
{
    class Logger
    {
        private Thread _Thread;
        private volatile Boolean _KeepWorking;
        public bool running = false;

        private string _log = "";
        private ListBox _outputBox;
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        public List<Rider> riders;
        public string apiVersion;

        public Logger(ListBox realBox)
        {
            _outputBox = realBox;
        }

        public string getLog()
        {
            return _log;
        }

        public void addLogMessage(string message, bool timeEncode = false)
        {
            if (timeEncode)
                message = message + " [ " + DateTime.Now + " ]";
            _log += message + "\n";

        }

        public void start(List<Rider> _riders, string _apiVersion)
        {
            riders = _riders;
            apiVersion = _apiVersion;
            _Thread = new Thread(worker);
            _KeepWorking = running = true;
            _Thread.Start();
        }

        public void stop()
        {
            _KeepWorking = running = false;
        }

        private void worker()
        {
            reset();
            Stopwatch runTime = Stopwatch.StartNew();
            while (_KeepWorking)
            {
                runTime = Stopwatch.StartNew();
                updateRiders();
                runTime.Stop();
                Thread.Sleep(Convert.ToUInt16(1000 - runTime.ElapsedMilliseconds));
            }
        }

        private void toBox(string message, bool timeStampLog = false)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate() { toBox(message, timeStampLog); });
            }
            else
            {
                addLogMessage(message, timeStampLog);
                _outputBox.Items.Add(message);
            }
        }

        public void reset()
        {
            clearBox();
            clearLog();
        }

        private void clearLog()
        {
            _log = "";
        }

        private void clearBox()
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate() { clearBox(); });
            }
            else
            {
                _outputBox.Items.Clear();
            }
        }

        public void updateRiders()
        {
            clearBox();
            switch (apiVersion)
            {
                case "0.8":
                    toBox("RPM  HR  PWR KCAL CLOCK SSI UUID              TSU", true);
                    foreach (Rider rider in riders)
                    {
                        toBox(rider.getString_v08());
                    }
                    break;
                case "1.0+":
                    toBox(" ID  RPM  HR  PWR  INT  CLOCK  KCAL  TRP  SSI  V  GR  UUID              TSU", true);
                    foreach (Rider rider in riders)
                    {
                        toBox(rider.getString_v10());
                    }
                    break;
            }

        }
    }
}
