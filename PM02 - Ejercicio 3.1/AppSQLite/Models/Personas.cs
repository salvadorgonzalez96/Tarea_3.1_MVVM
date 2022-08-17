using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AppSQLite.Models
{
    public class Personas
    {
        [PrimaryKey, AutoIncrement]
        public int code { get; set; }

        [MaxLength(70)]
        public String firstNames { get; set; }

        [MaxLength(70)]
        public String lastNames { get; set; }

        public int age { get; set; }

        [MaxLength(100)]
        public String address { get; set; }

        [MaxLength(100)]
        public String puesto { get; set; }

        public byte[] img { get; set; }


        public override string ToString()
        {
            return "Nombres: " + this.firstNames + " | Apellidos: " + this.lastNames + " | Edad: " + this.age;
        }

    }
}
