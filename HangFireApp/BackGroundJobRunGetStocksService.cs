﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireApp
{
    public class BackGroundJobRunGetStocksService
    {
        public static int counter=0;

        public void RunGetStocksService(string getStocksServicePath)
        {
            try
            {
                Process.Start(getStocksServicePath);
                Console.WriteLine($"GetStocksService {counter}. kere çalışıyor");
                Debug.WriteLine($"GetStocksService {counter}. kere çalışıyor");
                counter++;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FileNotFound or File is used");
            }
        }

    }
}