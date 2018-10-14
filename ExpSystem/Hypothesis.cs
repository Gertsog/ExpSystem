using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpSystem
{
    class Hypothesis
    {
        private string hypothesisName;
        private float defaultProbability;
        private float currentProbability;

        public string HypothesisName { get; set; }
        public float DefaultProbability { get; set; }
        public float CurrentProbability { get; set; }

        public Hypothesis(string hypothesisName, float defaultProbability)
        {
            HypothesisName = hypothesisName;
            DefaultProbability = defaultProbability;
            CurrentProbability = defaultProbability;
        }
    }
}