using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLiteDemo.Data;
using SQLiteDemo.Model;
using System.IO;

namespace SQLiteDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var rep = new SqLiteCustomerRepository();
            
            rep.DapperDemo();

           
        }
    }
}
