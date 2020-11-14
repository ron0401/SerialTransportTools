using System;
using CommandLine;
using System.IO.Ports;
using System.Net.Sockets;
using System.Collections.Generic;

namespace sttnet
{
    public class Transporter
    {
        [Option('p', "port", Required = true)]
        public string port { get; set; }
        [Option('u', "udp", Required = false)]
        public string udp { get; set; }        
        SerialPort srcPort = new SerialPort();
        
        List<UdpClient> UdpClientsList = new List<UdpClient>();

        public Transporter()
        {

        }
        public void Start()
        {
            genSrcSerialPort();
            genUdpPortsList();
        }
        void genSrcSerialPort()
        {
            srcPort.BaudRate = 9600;
            srcPort.PortName = this.port;
            srcPort.Open();
            srcPort.DataReceived += new SerialDataReceivedEventHandler(this.DataReceivedHandler);            
        }

        void genUdpPortsList()
        {
            string[] udpUnit = this.udp.Split(',');
            int i = 50000;
            foreach (var f in udpUnit)
            {
                string[] u = f.Split(':');
                var p = new UdpClient(i);
                p.Connect(u[0],int.Parse(u[1]));
                UdpClientsList.Add(p);
                i++;
            }
        }

        void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)
        {
            var sp = (SerialPort)sender;
            var str = sp.ReadLine();
            Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(str);
            foreach (var f in this.UdpClientsList)
            {
                f.Send(sendBytes,sendBytes.Length);
            }
        }
    }
}
