using System;
using System.Device.Gpio;

namespace RpiDotNet
{
    // Create the RangeFinder Class
    public class RangeFinder
    {
        private const int PIN_ECHO = 0;
        private const int PIN_TRIGGER = 1;

        private GpioController Gpio { get; } = new GpioController();

        public RangeFinder()
        {
            // Set the GPIO Pins
            Gpio.OpenPin(PIN_ECHO, PinMode.Input);
            Gpio.OpenPin(PIN_TRIGGER, PinMode.Output);
        }

        public double GetDistance()
        {
            // Send a 10us pulse to trigger
            Gpio.Write(PIN_TRIGGER, PinValue.High);
            Thread.Sleep (10);
            Gpio.Write(PIN_TRIGGER, PinValue.Low);

            // Wait for echo start
            while (Gpio.Read(PIN_ECHO) == PinValue.Low) ;

            // Wait for echo end
            var start = DateTime.Now;
            while (Gpio.Read(PIN_ECHO) == PinValue.High) ;
            var end = DateTime.Now;

            // Get distance in cm
            return  ((end - start).TotalSeconds / 58.2);
        }
    }
}