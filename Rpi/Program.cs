// See https://aka.ms/new-console-template for more information
using System.Device.Gpio;
using RpiDotNet;

internal class Program
{
    private static EnvironmentSensor _environment;
    private static GpioController _controller;
    private static WarningLight _warningLight;
    private static Timer _environmentTimer;

    private static async Task Main(string[] args)
    {
        Console.WriteLine("Application Start");

        int _LightPin = 18;
        int _switchPin = 17;
        int _environmentPin = 16;

        _controller = new GpioController(PinNumberingScheme.Logical);
        _warningLight = new WarningLight(_controller, _LightPin);
        _environment = new EnvironmentSensor(_controller, _environmentPin);
        _environmentTimer = new Timer (new TimerCallback (DataPostTimer));

        // Open the pin that connects to the magnetic swithch.
        _controller.OpenPin(_switchPin, PinMode.InputPullUp);
        _controller.RegisterCallbackForPinValueChangedEvent(
            _switchPin,
            PinEventTypes.Falling | PinEventTypes.Rising,
            OnPinEvent);

        // Put the main application into a perpetual wait state.
        await Task.Delay(Timeout.Infinite);

        
    }

    // THis method triggers on the interval to send data to the AWS IoT Service.
    // Each trigger should collect data for one message and send it.
    static void DataPostTimer(object? stateInfo)
    {
        var tempHumidity = _environment.GetConditions();
        if (tempHumidity != null)
        {
            Console.WriteLine("Conditions = " + tempHumidity.Temperature + "c - " + tempHumidity.Humidity + "%");
        }
        // Send data to AWS IoT
    }

    static void OnPinEvent(object sender, PinValueChangedEventArgs args)
    {
        if (args.ChangeType == PinEventTypes.Falling)
        {
            // Door is closed, we should send sensor data.
            Console.WriteLine("Door Closed");
            //_warningLight.SetWarning(false);
            // Trigger an almost immedate sending of data, then trigger every 10 seconds
            // While the door remains closed.
            _environmentTimer.Change(1000, 10000);
            
        }
        else
        {
            Console.WriteLine("Door Open");
            // Disable the environment timer while 
            if (_environmentTimer != null)
            {
                _environmentTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }
    }
}