using Fejlesztesi_mintak.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fejlesztesi_mintak.Abstractions
{
    public interface IToyFactory
    {
        Toy CreateNew();
    }
}
