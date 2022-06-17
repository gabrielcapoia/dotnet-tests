using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.BDD.Tests.Config
{
    public static class TestsExtensions
    {
        public static int ApenasNumeros(this string value)
        {
            return Convert.ToInt16(new string(value.Where(char.IsDigit).ToArray()));
        }
    }
}
