using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Observers
{
    public class OBSStatus : IOBSStatus
    {
        private readonly Random random = new Random();
        public long Time => random.Next();
    }
}
