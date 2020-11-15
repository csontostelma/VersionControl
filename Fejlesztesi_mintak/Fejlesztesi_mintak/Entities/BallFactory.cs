using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fejlesztesi_mintak.Entities
{
    public class BallFactory
    {
        public Ball CreateNew()
        {
            return new Ball();
        }
    }
}
