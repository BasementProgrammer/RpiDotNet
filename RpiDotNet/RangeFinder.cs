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
            Gpio.PinMode(PIN_ECHO, Gpio.INPUT);
            Gpio.PinMode(PIN_TRIGGER, Gpio.OUTPUT);
        }

        public int GetDistance()
        {
            // Send a 10us pulse to trigger
            Gpio.DigitalWrite(PIN_TRIGGER, Gpio.HIGH);
            Gpio.DelayMicroseconds(10);
            Gpio.DigitalWrite(PIN_TRIGGER, Gpio.LOW);

            // Wait for echo start
            while (Gpio.DigitalRead(PIN_ECHO) == Gpio.LOW) ;

            // Wait for echo end
            var start = Gpio.Micros();
            while (Gpio.DigitalRead(PIN_ECHO) == Gpio.HIGH) ;
            var end = Gpio.Micros();

            // Get distance in cm
            return (int)((end - start) / 58.2);
        }
    }
}