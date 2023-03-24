using System.Device.Gpio;

namespace RpiDotNet
{
    public class WarningLight
    {
        private GpioController _controller;
        private bool _lightOn = false;
        private int _pinNumber;

        private bool _enableWarning = false;

        private Thread _warningLightThread;
        public WarningLight (GpioController controller, int pinNumber)
        {
            _controller = controller;
            _controller.OpenPin (pinNumber, PinMode.Output);
            _pinNumber = pinNumber;
        }

        public void BlinkLight ()
        {
            _lightOn = !_lightOn;
            _controller.Write (_pinNumber, ((_lightOn) ? PinValue.High : PinValue.Low));
        }

        private void DoWarningLight ()
        {
            while (true)
            {
                BlinkLight ();
                Thread.Sleep (1000);
            }
        }

        public void SetWarning (bool enable)
        {
            _enableWarning = enable;
            if (enable)
            {
                _warningLightThread = new Thread (new ThreadStart (DoWarningLight));
                _warningLightThread.Start ();
            }
            
        }
    }
}