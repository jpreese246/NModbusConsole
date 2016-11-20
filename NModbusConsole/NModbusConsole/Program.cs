using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using Modbus.Device;
using System.Diagnostics;

namespace ChamberView
{
    class SampleClass
    {
        static void Main()
        {
            string whif = "COM6";
            ModbusSerialRtuMasterWriteRegisters(whif);
            ModbusSerialRtuMasterReadRegisters(whif);
            printPorts();
        }
        private static void ModbusSerialRtuMasterWriteRegisters(string baudBaud)
        {

            //MessageBox.Show(baudBaud);
            using (SerialPort port = new SerialPort(baudBaud))
            {
                // configure serial port
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                // create modbus master
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                //ushort[] ReadHoldingRegisters(
                byte slaveId = 1;
                ushort startAddress = 300;
                ushort value = 670;
                master.WriteSingleRegister(slaveId, startAddress, value);
                //ushort[] holding_register = master.ReadHoldingRegisters(slaveId, startAddress, 1);
            }

        }


        public static void printPorts()
        {
            var portNames = SerialPort.GetPortNames();

            foreach (var p in portNames)
            {
                Console.WriteLine(p);
            }
        }

        public static void ModbusSerialRtuMasterReadRegisters(string baudBaud)
        {
            using (SerialPort port = new SerialPort(baudBaud))
            {
                // configure serial port

                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;

                try
                {
                    port.Open();
                    Console.WriteLine("port " + port.PortName + " open: " + port.IsOpen + "\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to open port: " + ex);
                }

                // create modbus master
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                byte slaveId = 1;
                ushort startAddress = 300;
                ushort numRegisters = 5;
                ushort[] registers = new ushort[numRegisters]; ;

                // read registers
                try
                {
                    registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
                    for (int i = 0; i < numRegisters; i++)
                        Console.WriteLine("Register {0}={1}", startAddress + i, registers[i]);
                }
                catch (Modbus.SlaveException se)
                {
                    Console.WriteLine("Could not find register... \n \n" + se);
                }

                try
                {
                    port.Close();
                    Console.WriteLine("\nport " + port.PortName + " open: " + port.IsOpen + "\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to close port: " + ex);
                }
            }

            // output:
            // Register 1=0
            // Register 2=0
            // Register 3=0
            // Register 4=0
            // Register 5=0
        }

    }

}