using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Const
{
    public static class CommonConstant
    {
        public const string ConnectionString = "Data Source=DESKTOP-V64DOTK;Initial Catalog=CoreDb;Integrated Security=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    }
    public static class UserStatus
    {
        public const int Locked = 0;
        public const int Working = 1;
    }
}
