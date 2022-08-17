using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AppSQLite.Models;
using SQLite;


namespace AppSQLite.Controller
{
    public class Database
    {
        readonly SQLiteAsyncConnection db;

        public Database(String pathdb)
        {
            db = new SQLiteAsyncConnection(pathdb);
            // Creacion de tabla personas 
            db.CreateTableAsync<Models.Personas>().Wait();
        }

        // CRUD operations with SQLite
        public Task<List<Personas>> getListPersons()
        {
            return db.Table<Personas>().ToListAsync();
        }

        // Get person by code
        public Task<Personas> getPersonByCode(int codeParam)
        {
            return db.Table<Personas>()
                .Where(i => i.code == codeParam)
                .FirstOrDefaultAsync();
        }

        // Create person
        public Task<int> savePerson(Personas person)
        {
            if(person.code !=0)
            {
                return db.UpdateAsync(person);
            } 
            else
            {
                return db.InsertAsync(person);
            }

        }

        public Task<int> deletePerson(Personas person)
        {
            return db.DeleteAsync(person);
        }

        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
