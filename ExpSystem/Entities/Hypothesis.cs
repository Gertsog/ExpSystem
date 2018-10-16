using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpSystem
{
    public class Hypothesis
    {
        private string hypothesisName;
        private double defaultProbability;
        private double currentProbability;

        public string HypothesisName { get; set; }
        public double DefaultProbability { get; set; }
        public double CurrentProbability { get; set; }

        public List<int> questionNumbers = new List<int>();
        public List<double> pPositive = new List<double>();
        public List<double> pNegative = new List<double>();

        public Hypothesis()
        {

        }

        public Hypothesis(string hypothesisName, double defaultProbability)
        {
            HypothesisName = hypothesisName;
            DefaultProbability = defaultProbability;
            CurrentProbability = defaultProbability;
        }
    }
}