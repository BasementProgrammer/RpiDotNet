// See https://aka.ms/new-console-template for more information
using System.Device.Gpio;
using RpiDotNet;

Console.WriteLine("Hello, World!");

GpioController _controller = new GpioController (PinNumberingScheme.Logical);

WarningLight light = new WarningLight (_controller, 18);

EnvironmentSensor environment = new EnvironmentSensor (_controller, 16);

//light.SetWarning (true);

while (true)
{
    double temp = environment.GetTemperature ();
    light.BlinkLight ();

    if ( temp > 0)
    {
        Console.WriteLine (temp + "c");
    }
    else
    {
        Console.Write (".");
    }

    Thread.Sleep (1000);
}