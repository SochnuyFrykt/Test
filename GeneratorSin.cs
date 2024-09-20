using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    internal class GeneratorSin
    {
        public List<double> Data { get; private set; } = new List<double>();
        private int index = 0;
        private double amplitude { get; set; }
        private double frequency { get; set; }

        public GeneratorSin(double amplitude, double frequency)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
        }

        public async Task GenerateDataAsync(CancellationToken token)
        {
            int head = 50;
            while (!token.IsCancellationRequested)
            {
                for (int i = 0; i < head; i++)
                    Data.Add(amplitude * Math.Sin(frequency * (index + i) * 0.1));
                index += head;
                try
                {
                    await Task.Delay(60, token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
    }
}
