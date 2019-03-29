using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class User
    {
        public string Name { get; set; }
        public RSAEncryptor rsaEncryptor { get; set; }
    }
}
