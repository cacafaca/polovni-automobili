﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procode.PolovniAutomobili.Data.Provider
{
    public static class DataInstance
    {
        private static Data data;
        public static Data Data { get { return data; } }
        static DataInstance()
        {
            data = Data.GetNewDataInstance();
        }
    }
}
