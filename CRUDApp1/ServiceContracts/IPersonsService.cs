using System;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new person to the system based on the specified request data.
        /// </summary>
        /// <param name="request">An object containing the details of the person to add. Cannot be null.</param>
        /// <returns>A response object containing information about the added person, including the assigned identifier and
        /// status of the operation.</returns>
        PersonResponse AddPerson(PersonAddRequest request);

        /// <summary>
        /// Retrieves a list of persons represented by PersonResponse objects.
        /// </summary>
        /// <returns>A list of PersonResponse objects containing information about each person. The list is empty if no persons
        /// are found.</returns>
        List<PersonResponse> GetPersonList();


        /// <summary>
        /// Retrieves the details of a person with the specified unique identifier.
        /// </summary>
        /// <param name="personID">The unique identifier of the person to retrieve.</param>
        /// <returns>A PersonResponse object containing the person's details if found; otherwise, null.</returns>
        PersonResponse GetPersonByID(Guid personID);
    }
}
