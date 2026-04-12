using System;
using System.Linq.Expressions;
using ContactsManager.Domain.Entities;

namespace ContactsManager.Domain.RepositoryContracts
{
    public interface IPersonsRespository
    {
        /// <summary>
        /// Takes  a person
        /// </summary>
        /// <param name="person"></param>
        /// <returns>The added person</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Takes no input
        /// </summary>
        /// <returns>List of all Persons</returns>
        Task<List<Person>?> GetAllPersons();

        /// <summary>
        /// Takes Person ID Guid
        /// </summary>
        /// <param name="PersonID"></param>
        /// <returns>The Person if found or null if not found</returns>
        Task<Person?> GetPersonByPersonID(Guid? PersonID);

        /// <summary>
        /// Asynchronously retrieves a list of persons that satisfy the specified filter criteria.
        /// </summary>
        /// <param name="predicate">An expression that defines the conditions each person must meet to be included in the result. Cannot be
        /// null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of persons matching the
        /// filter, or null if no persons are found.</returns>
        Task<List<Person>?> GetFilteredPersons(Expression<Func<Person,bool>> predicate);

        /// <summary>
        /// Deletes the person record associated with the specified person identifier.
        /// </summary>
        /// <param name="PersonID">The unique identifier of the person to delete. If null, the operation will not be performed.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the
        /// person was successfully deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeletePersonByPersonID(Guid? PersonID);

        /// <summary>
        /// Updates the person record identified by the specified person ID.
        /// </summary>
        /// <param name="PersonID">The unique identifier of the person to update. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated person if the update
        /// is successful; otherwise, null if no person with the specified ID exists.</returns>
        Task<Person?> UpdatePersonByPersonID(Person? Person); 
    }
}
