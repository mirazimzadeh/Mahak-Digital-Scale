using System;
using System.IO.Ports;
using System.Linq;

namespace MahakDigitalScale
{
    class Program
    {
        private static SerialPort _serialPort;
        private static long _weight;
        static void Main()
        {
            _serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            _serialPort.DataReceived += port_DataReceived;
            _serialPort.Open();
            while (true)
            {
                Console.ReadLine();
            }
            // ReSharper disable once FunctionNeverReturns
        }

        static void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = new byte[_serialPort.BytesToRead];
            _serialPort.Read(data, 0, data.Length);

            var hex = data.Aggregate("", (current, q) => current + string.Format("{0:X2}", q));
            hex = hex.Replace("00", "");

            try
            {
                var local = Convert.ToInt64(hex, 16);
                if (local < 50000) _weight = local;
                Console.WriteLine("weight : {0} g" , _weight );
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}

