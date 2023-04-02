

using System;
using System.Device.Gpio;
using UnitsNet;

namespace RpiDotNet
{
    public class EnvironmentSensor
    {
        private Iot.Device.DHTxx.Dht11 _sensor;
        private GpioController _controller;
        private int _pinNumber;
        public EnvironmentSensor (GpioController controller, int pinNumber)
        {
            _controller = controller;
            _pinNumber = pinNumber;

            //controller.OpenPin (_pinNumber, PinMode.Input);
            _sensor = new Iot.Device.DHTxx.Dht11 (_pinNumber, PinNumberingScheme.Logical, _controller, false);
        }

        public double GetTemperature ()
        {
            Temperature temp = new Temperature ();
            
            if (_sensor.TryReadTemperature (out temp))
            {
                return temp.DegreesCelsius;
            }
            else
            {
                
                return -1;
            }
        }
    }
}