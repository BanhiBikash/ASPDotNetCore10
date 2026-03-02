using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

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
        PersonResponse? GetPersonByID(Guid? personID);

        /// <summary>
        /// Retrieves a list of persons filtered by a specified property and value.
        /// </summary>
        /// <param name="ByProperty">The name of the property to filter by. This value is case-insensitive. If null or empty, no filtering is
        /// applied.</param>
        /// <param name="PropertyValue">The value to match for the specified property. If null or empty, no filtering is applied.</param>
        /// <returns>A list of persons that match the specified filter criteria. Returns all persons if no filter is applied. The
        /// list will be empty if no persons match the criteria.</returns>
        List<PersonResponse> GetFilteredPersons(string? ByProperty, string? PropertyValue);

        /// <summary>
        /// Returns a list of persons sorted by the specified property.
        /// </summary>
        /// <param name="ByProperty">The name of the property to sort by. If null or empty, the default sort order is applied. Supported property
        /// names are case-insensitive.</param>
        /// <returns>A list of <see cref="PersonResponse"/> objects sorted according to the specified property. The list will be
        /// empty if there are no persons to return.</returns>
        List<PersonResponse> GetSortedPersons(string? ByProperty, bool ascending = true);
    }
}
