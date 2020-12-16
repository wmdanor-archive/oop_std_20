using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Task4
{
    public class TemperatureController
    {
        private bool _running;
        private bool _toStopRunning;

        private Task _changingTask;
        private Action _runTemperatureChanger;

        private double _setTemperature;
        private double _currentTemperature;

        public TemperatureController(double setTemperature)
        {
            _runTemperatureChanger = RunTemperatureChanger;

            Random random = new Random();
            _currentTemperature = random.NextDouble() * (28 - 18) + 18;

            SetTemperature = setTemperature;
        }

        public double CurrentTemperature { get => _currentTemperature; private set => _currentTemperature = value; }
        public double SetTemperature
        {
            get
            {
                return _setTemperature;
            }

            set
            {
                if (value == _setTemperature) return;

                _setTemperature = value;

                if (value == CurrentTemperature)
                {
                    if (_running)
                    {
                        StopTemperatureChanger();
                        return;
                    }
                    else return;
                }

                if (_running)
                {
                    StopTemperatureChanger();
                    _changingTask.Wait();
                    _changingTask = Task.Run(_runTemperatureChanger);
                }
                else
                {
                    if (_changingTask != null) _changingTask.Wait();
                    _changingTask = Task.Run(_runTemperatureChanger);
                }
            }
        }

        private void RunTemperatureChanger()
        {
            _running = true;

            double deltaTemperature = _setTemperature - CurrentTemperature;

            // just work imitation next

            int times = (int)(Math.Abs(deltaTemperature) / 0.05);
            double dt = deltaTemperature >= 0 ? 0.05 : -0.05;

            for (int i = 0; i < times && !_toStopRunning; i++)
            {
                Thread.Sleep(100);
                Interlocked.Exchange(ref _currentTemperature, _currentTemperature + dt);
            }

            _running = false;
            _toStopRunning = false;
        }

        private void StopTemperatureChanger()
        {
            if (!_running) return;

            _toStopRunning = true;
        }
    }

    public class HumidityController
    {
        private bool _running;
        private bool _toStopRunning;

        private Task _changingTask;
        private Action _runHumidityChanger;

        private double _setHumidity;
        private double _currentHumidity;

        public HumidityController(double setHumidity)
        {
            _runHumidityChanger = RunHumidityChanger;

            Random random = new Random();
            _currentHumidity = random.NextDouble() * (15 - 5) + 5;

            SetHumidity = setHumidity;
        }

        public double CurrentHumidity { get => _currentHumidity; private set => _currentHumidity = value; }
        public double SetHumidity
        {
            get
            {
                return _setHumidity;
            }

            set
            {
                if (value == _setHumidity) return;

                _setHumidity = value;

                if (value == CurrentHumidity)
                {
                    if (_running)
                    {
                        StopHumidityChanger();
                        return;
                    }
                    else return;
                }

                if (_running)
                {
                    StopHumidityChanger();
                    _changingTask.Wait();
                    _changingTask = Task.Run(_runHumidityChanger);
                }
                else
                {
                    if (_changingTask != null) _changingTask.Wait();
                    _changingTask = Task.Run(_runHumidityChanger);
                }
            }
        }

        private void RunHumidityChanger()
        {
            _running = true;

            double deltaHumidity = _setHumidity - CurrentHumidity;

            // just work imitation next

            int times = (int)(Math.Abs(deltaHumidity) / 0.05);
            double dh = deltaHumidity >= 0 ? 0.05 : -0.05;

            for (int i = 0; i < times && !_toStopRunning; i++)
            {
                Thread.Sleep(100);
                Interlocked.Exchange(ref _currentHumidity, _currentHumidity + dh);
            }

            _running = false;
            _toStopRunning = false;
        }

        private void StopHumidityChanger()
        {
            if (!_running) return;

            _toStopRunning = true;
        }
    }

    public class ClimateController
    {
        private Dictionary<string, ClimateMode> _climateModes = new Dictionary<string, ClimateMode>();
        private TemperatureController _temperatureController;
        private HumidityController _humidityController;

        public ClimateMode CurrentMode { get; private set; }

        public ClimateController(ClimateMode currentState)
        {
            CurrentMode = currentState;
            _temperatureController = new TemperatureController(currentState.Temperature);
            _humidityController = new HumidityController(currentState.Humidity);
        }

        public ClimateMode CurrentState
        {
            get
            {
                return new ClimateMode(_temperatureController.CurrentTemperature, _humidityController.CurrentHumidity);
            }
        }

        public void Add(string name, ClimateMode climateMode)
        {
            _climateModes.Add(name, climateMode);
        }

        public bool Remove(string name)
        {
            return _climateModes.Remove(name);
        }

        public bool ChooseMode(string name)
        {
            ClimateMode climateMode;

            if (!_climateModes.TryGetValue(name, out climateMode)) return false;

            CurrentMode = climateMode;
            _temperatureController.SetTemperature = climateMode.Temperature;
            _humidityController.SetHumidity = climateMode.Humidity;

            return true;
        }
    }

    public class ClimateMode
    {
        public double Temperature { get; }
        public double Humidity { get; }

        public ClimateMode(double temperature, double humidity)
        {
            Temperature = temperature;
            Humidity = humidity;
        }

        public override string ToString()
        {
            return "Temperature = " + Temperature.ToString() + ", humidity = " + Humidity.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var controller = new ClimateController(new ClimateMode(23, 10));
            controller.Add("colder", new ClimateMode(20, 5));

            Console.WriteLine(controller.CurrentState);

            while (true)
            {
                if (Console.ReadKey().KeyChar == 'q') break;

                Console.WriteLine(controller.CurrentState);
            }

            Console.WriteLine("Setting t = 20, h = 5");
            controller.ChooseMode("colder");

            while (true)
            {
                if (Console.ReadKey().KeyChar == 'q') break;

                Console.WriteLine(controller.CurrentState);
            }
        }
    }
}
