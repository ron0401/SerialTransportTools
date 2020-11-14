using System;
using CommandLine;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sttnet
{
    public class Transporter
    {
        [Option('p', "port", Required = true)]
        public string port { get; set; }
        [Option('u', "udp", Required = false)]
        public string udp { get; set; }        
        [Option('d', "delimiter", Required = false)]
        public string delimiter { get; set; }      
        [Option('b', "debug",Default = false ,Required = false)]
        public bool debug { get; set; }              
        [Option('l', "listen",Required = false)]
        public int listen { get; set; }         
        SerialPort srcPort = new SerialPort();
        
        List<UdpClient> UdpClientsList = new List<UdpClient>();
        UdpClient listenPort;

        public Transporter()
        {

        }
        public void Start()
        {
            genSrcSerialPort();
            genUdpPortsList();
            if (this.delimiter == null)
            {
                this.delimiter = System.Environment.NewLine;
            }
            if (this.listen != 0)
            {
                listenPort = new UdpClient(this.listen);
                var t = Task.Factory.StartNew(UdpRecieve);
            }
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
            var str = sp.ReadTo(this.delimiter);

            if (this.debug)
            {
                Console.WriteLine("Serial Recieved: " + str);
            }
            Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(str + this.delimiter);
            foreach (var f in this.UdpClientsList)
            {
                f.Send(sendBytes,sendBytes.Length);
            }
        }

        void UdpRecieve()
        {
            var ip = new IPEndPoint(IPAddress.Any, 0);    
            while (true)
            {
                Byte[] receiveBytes = this.listenPort.Receive(ref ip);
                if (this.debug)
                {
                    Console.WriteLine("UDP Recieved: " + System.Text.Encoding.ASCII.GetString(receiveBytes));
                }
                srcPort.Write(receiveBytes,0,receiveBytes.Length);
            }
        }
    }
}
