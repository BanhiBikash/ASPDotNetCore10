using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text.Json;

namespace Entities
{
    public class PersonsDBContext:DbContext
    {
        public PersonsDBContext(DbContextOptions options): base(options)
        {

        }

        //person type of data
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().ToTable("Persons");

            //fetching data from json file
            string personsJson = System.IO.File.ReadAllText("Data/Persons.json");

            //deserializing json data and seeding it to the database
            List<Person>? persons = JsonSerializer.Deserialize<List<Person>>(personsJson);
                
            foreach(var person in persons)
            {
                if(person != null)
                modelBuilder.Entity<Person>().HasData(person);
            };
        }

        //stored procedure to get all persons from the database
        public List<Person> getAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public List<Person> SortPersonsByProperty(string propertyName)
        {
            return Persons
                .FromSqlRaw("EXECUTE [dbo].[SortPersonsByProperty] @propertyName",
                            new Microsoft.Data.SqlClient.SqlParameter("@propertyName", propertyName))
                .ToList();
        }

    }
}
