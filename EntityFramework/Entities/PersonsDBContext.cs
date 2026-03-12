using Microsoft.Data.SqlClient;
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

        public int InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@CountryID", person.CountryID)
            };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @Address, @CountryID", parameters);
        }

        public int DeletePerson(Guid? PersonID)
        {
            SqlParameter parameter = new SqlParameter("@PersonID", PersonID);

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[DeletePerson] @PersonID", parameter);
        }
        public int EditPerson(Person person) 
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@CountryID", person.CountryID)
            };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[EditPerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @Address, @CountryID", parameters);
        }

        public List<Person> FilteredPersons(string personProperty, string propertyValue)
        {
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter("@personProperty", personProperty),
                new SqlParameter("@propertyValue", propertyValue)
            };

            return Persons.FromSqlRaw("EXECUTE [dbo].[FilteredPersons] @personProperty, @propertyValue", parameters).ToList();
        }
    }
}
