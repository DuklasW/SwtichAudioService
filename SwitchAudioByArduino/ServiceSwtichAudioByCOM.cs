using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO.Ports;
using AudioSwitcher.AudioApi;

namespace SwitchAudioByArduino
{
    public partial class ServiceSwtichAudioByCOM : ServiceBase
    {
        private CoreAudioController audioController;
        private SerialPort serialPort;
        private static readonly string portCom = ConfigurationManager.AppSettings["PortCOM"];
        private static readonly int customBautRate = int.Parse(ConfigurationManager.AppSettings["BautRate"]);
        private EventLog eventLog;

        public ServiceSwtichAudioByCOM()
        {
            InitializeComponent();
            eventLog= new EventLog();
            if (!EventLog.SourceExists("SwitchAudioByArduino"))
            {
                EventLog.CreateEventSource("SwitchAudioByArduino", "SwitchAudioByArduinoLog");
            }
            eventLog.Source = "SwitchAudioByArduino";
            eventLog.Log = "SwitchAudioByArduinoLog";
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                audioController = new CoreAudioController();
                serialPort= new SerialPort(portCom, customBautRate);
                serialPort.DataReceived += SerialPortDataRecived;
                serialPort.Open();
                eventLog.WriteEntry("Service started successfully.", EventLogEntryType.SuccessAudit);
            }
            catch(Exception ex) 
            {
                eventLog.WriteEntry($"Error occurred while starting the service: {ex.Message}", EventLogEntryType.Error);
                throw;
            }
           
        }

        private void SerialPortDataRecived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort.ReadLine();
                if (data.Trim().Equals("SWITCH"))
                {
                    SwitchToNextAudioDevice();
                }
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry($"Error occurred during data reception: {ex.Message}", EventLogEntryType.Error);
            }
        }

        private void SwitchToNextAudioDevice()
        {
            try
            {
                var devices = audioController.GetPlaybackDevices();
                var defaultDevice = audioController.DefaultPlaybackDevice;

                var activeDevices = devices.Where(device => device.State == DeviceState.Active).ToList();
                if (activeDevices.Count == 0)
                {
                    eventLog.WriteEntry("No active playback devices found.", EventLogEntryType.Warning);
                    return;
                }


                int currentIndex = activeDevices.IndexOf(defaultDevice);
                if (currentIndex == -1)
                {
                    eventLog.WriteEntry("Default playback device not found among active devices.", EventLogEntryType.Error);
                    return;
                }

                int nextDeviceIndex = (currentIndex + 1) % activeDevices.Count;
                var nextDevice = activeDevices[nextDeviceIndex];
                nextDevice.SetAsDefault();

            }
            catch (Exception ex)
            {
                eventLog.WriteEntry($"Error occurred while switching audio device: {ex.Message}", EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            try
            {
                serialPort.Close();
                eventLog.WriteEntry("Service stopped successfully.", EventLogEntryType.SuccessAudit);
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry($"Error occurred while stopping the service: {ex.Message}", EventLogEntryType.Error);
            };
        }
    }
}
