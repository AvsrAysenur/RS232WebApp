using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace RS232WebApp.Models // RS232Service sınıfını bir namespace içine alıyoruz
{
    public class RS232Service
    {
        private readonly SerialPort _port1;
        private readonly SerialPort _port2;

        public RS232Service(string portName1, int baudRate1, string portName2, int baudRate2)
        {
            _port1 = new SerialPort(portName1, baudRate1);
            _port2 = new SerialPort(portName2, baudRate2);

            // Portları aç
            _port1.Open();
            _port2.Open();

            // Her iki port için dinleme işlemini başlat
            Task.Run(() => ListenToPort(_port1, _port2));
            Task.Run(() => ListenToPort(_port2, _port1));
        }

        public void SendMessageToPort1(string message)
        {
            if (_port1.IsOpen)
            {
                _port1.WriteLine(message);
            }
        }

        public void SendMessageToPort2(string message)
        {
            if (_port2.IsOpen)
            {
                _port2.WriteLine(message);
            }
        }

        public string ReceiveMessageFromPort1()
        {
            if (_port1.IsOpen)
            {
                return _port1.ReadLine();
            }
            return string.Empty;
        }

        public string ReceiveMessageFromPort2()
        {
            if (_port2.IsOpen)
            {
                return _port2.ReadLine();
            }
            return string.Empty;
        }

        private static void ListenToPort(SerialPort receivingPort, SerialPort sendingPort)
        {
            while (true)
            {
                try
                {
                    if (receivingPort.IsOpen)
                    {
                        var message = receivingPort.ReadLine();
                        if (sendingPort.IsOpen)
                        {
                            sendingPort.WriteLine(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda gerekli işlemleri yapın
                    Console.WriteLine($"Error in listening to port: {ex.Message}");
                }
            }
        }

        public void ClosePorts()
        {
            if (_port1.IsOpen)
            {
                _port1.Close();
            }
            if (_port2.IsOpen)
            {
                _port2.Close();
            }
        }
    }
}
