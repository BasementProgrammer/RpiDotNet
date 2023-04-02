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
    //double temp = environment.GetTemperature ();
    var conditions = environment.GetConditions ();

    light.BlinkLight ();

    if ( conditions !=  null)
    {
        Console.WriteLine (conditions.Temperature + "c - " + conditions.Humidity + "%");
    }
    else
    {
        Console.Write (".");
    }

    Thread.Sleep (1000);
}