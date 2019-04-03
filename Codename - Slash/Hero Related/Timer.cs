using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    // Useful helper class, for keeping time 
    public class Timer
    {
        public bool Running { get; set; }

        private float speed;
        private float maxTimerValue;
        private float currentTimerValue;

        // 
        public Timer(float maxTimerValue, float speed = 1)
        {
            this.maxTimerValue = maxTimerValue;
            this.speed = speed;
        }

        // Simulates timer 
        public void Update(float deltaTime)
        {
            // check
            if (Running)
            {
                // Increment timer
                currentTimerValue += speed * deltaTime;
                // Reset timer if over
                if (currentTimerValue >= maxTimerValue)
                {
                    Stop();
                }
            }
        }

        // Start Timer
        public void Start()
        {
            Running = true;
            currentTimerValue = 0.0f;
        }

        // Stop Timer
        public void Stop()
        {
            Running = false;
            currentTimerValue = 0.0f;
        }

    }
}
