using DataContextPerfume;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            BD db = new BD();
            db.Database.Log = GravaLog;

            Console.WriteLine("ok");

            Console.Read();
        }

        public static void GravaLog(string sql)
        {
            Console.WriteLine("Comando SQL: " + sql);
        }
    }
}
