using business;
using DataContextPerfume;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            BD db = new BD();
            db.Database.Log = GravaLog;

            Perfume p1 = new Perfume("Ella");
            Perfume p2 = new Perfume("Rebelle");
            Perfume p3 = new Perfume("Empire");

            p1.Preco = 129.90m;
            p1.Marca = "Hinode10";
            p1.Fragrancia = new Fragrancia();
            p1.Fragrancia.NumeroFragrancia = 7;
            p1.Fragrancia.DescricaoFragrancia = "leve";
            p2.Preco = 99.90m;
            p2.Marca = "Hinode11";
            p2.Fragrancia = new Fragrancia();
            p2.Fragrancia.NumeroFragrancia = 8;
            p2.Fragrancia.DescricaoFragrancia = "Forte";
            p3.Preco = 29.90m;
            p3.Marca = "Hinode12";
            p3.Fragrancia = new Fragrancia();
            p3.Fragrancia.NumeroFragrancia = 9;
            p3.Fragrancia.DescricaoFragrancia = "Médio";

            p1.Estoque = 10;
            p2.Estoque = 10;
            p3.Estoque = 10;

            db.Produto.Add(p1);
            db.Produto.Add(p2);
            db.Produto.Add(p3);

            db.SaveChanges();

            Console.Read();
        }

        public static void GravaLog(string sql)
        {
            Console.WriteLine("Comando SQL: " + sql);
        }
    }
}
