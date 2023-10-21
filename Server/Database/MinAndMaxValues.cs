using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClientServerApp.Database
{
    internal class MinAndMaxValues
    {
        private int max = 1;
        private int min = int.MinValue;

        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        public int Min
        {
            get { return min; }
            set { min = value; }
        }

        public MinAndMaxValues(int min, int max)
        {
            this.max = max;
            this.min = min;
        }
        public MinAndMaxValues()
        { }

        public MinAndMaxValues GroupIDGetMaxAndMin(List<Group> Groups)
        {
            MinAndMaxValues MinAndMaxValues = new MinAndMaxValues();
            int min = int.MaxValue;
            int max = -1;

            foreach (Group group in Groups)
            {
                if (group.id > max) max = group.id;
                if (group.id < min) min = group.id;
            }
            MinAndMaxValues.Min = min;
            MinAndMaxValues.Max = max;
            return MinAndMaxValues;
        }
        public MinAndMaxValues StatusIDGetMaxAndMin(List<LearningStatus> Statuses)
        {
            MinAndMaxValues MinAndMaxValues = new MinAndMaxValues();

            foreach (LearningStatus status in Statuses)
            {
                if (status.id > max) max = status.id;
                if (status.id < min) min = status.id;
            }
            MinAndMaxValues.Min = min;
            MinAndMaxValues.Max = max;

            return MinAndMaxValues;
        }
    }
}
