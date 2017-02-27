using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenser
{
    class Program
    {
        static void Main(string[] args)
        {
            string hashResult = Encryption.Hash("Chris84948@gmail.com", Encryption.GetSalt());
        }
    }
}
