using System;
using System.Collections.Generic;
using System.Threading;

namespace Test
{
    internal class GeneratorSin
    {
        public List<double> Data { get; private set; } = new List<double>();
        private int index = 0;
        private double amplitude { get; set; }
        private double frequency { get; set; }
        private bool isRunning = true;


        public GeneratorSin(double amplitude, double frequency)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
        }

        public void Generate()
        {
            while (isRunning)
            {
                for (int i = 0; i < 10; i++)
                {
                    Data.Add(amplitude * Math.Sin(frequency * (index + i) * 0.1));
                }
                index += 10;
                Thread.Sleep(50);
            }
        }

        public void Stop()
        {
            isRunning = false;
        }
    }
}
