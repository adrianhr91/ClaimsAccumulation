﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = Parser.ParseClaimsData();

            var values = data.AccumulateValues("House");
        }
    }
}
