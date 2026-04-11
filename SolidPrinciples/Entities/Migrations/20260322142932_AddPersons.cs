using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddPersons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = "INSERT INTO PersonsData (PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters)\r\nVALUES\r\n('c03bbe45-9aeb-4d24-99e0-4743016ffce9', 'Marguerite', 'mwebsdale0@people.com.cn', '1989-08-28', 'Female', '56bf46a4-02b8-4693-a0f5-0a95e2218bdc', '4 Parkside Point', 0),\r\n('c3abddbd-cf50-41d2-b6c4-cc7d5a750928', 'Ursa', 'ushears1@globo.com', '1990-10-05', 'Female', '14629847-905a-4a0e-9abe-80b61655c5cb', '6 Morningstar Circle', 0),\r\n('c6d50a47-f7e6-4482-8be0-4ddfc057fa6e', 'Franchot', 'fbowsher2@howstuffworks.com', '1995-02-10', 'Male', '14629847-905a-4a0e-9abe-80b61655c5cb', '73 Heath Avenue', 1),\r\n('d15c6d9f-70b4-48c5-afd3-e71261f1a9be', 'Angie', 'asarvar3@dropbox.com', '1987-01-09', 'Male', '12e15727-d369-49a9-8b13-bc22e9362179', '83187 Merry Drive', 1),\r\n('89e5f445-d89f-4e12-94e0-5ad5b235d704', 'Tani', 'ttregona4@stumbleupon.com', '1995-02-11', 'Gender', '56bf46a4-02b8-4693-a0f5-0a95e2218bdc', '50467 Holy Cross Crossing', 0),\r\n('2a6d3738-9def-43ac-9279-0310edc7ceca', 'Mitchael', 'mlingfoot5@netvibes.com', '1988-01-04', 'Male', '8f30bedc-47dd-4286-8950-73d8a68e5d41', '97570 Raven Circle', 0),\r\n('29339209-63f5-492f-8459-754943c74abf', 'Maddy', 'mjarrell6@wisc.edu', '1983-02-16', 'Male', '12e15727-d369-49a9-8b13-bc22e9362179', '57449 Brown Way', 1),\r\n('ac660a73-b0b7-4340-abc1-a914257a6189', 'Pegeen', 'pretchford7@virginia.edu', '1998-12-02', 'Female', '12e15727-d369-49a9-8b13-bc22e9362179', '4 Stuart Drive', 1),\r\n('012107df-862f-4f16-ba94-e5c16886f005', 'Hansiain', 'hmosco8@tripod.com', '1990-09-20', 'Male', '12e15727-d369-49a9-8b13-bc22e9362179', '413 Sachtjen Way', 1),\r\n('cb035f22-e7cf-4907-bd07-91cfee5240f3', 'Lombard', 'lwoodwing9@wix.com', '1997-09-25', 'Male', '8f30bedc-47dd-4286-8950-73d8a68e5d41', '484 Clarendon Court', 0),\r\n('28d11936-9466-4a4b-b9c5-2f0a8e0cbde9', 'Minta', 'mconachya@va.gov', '1990-05-24', 'Female', '501c6d33-1bbe-45f1-8fbd-2275913c6218', '2 Warrior Avenue', 1),\r\n('a3b9833b-8a4d-43e9-8690-61e08df81a9a', 'Verene', 'vklussb@nationalgeographic.com', '1987-01-19', 'Female', '501c6d33-1bbe-45f1-8fbd-2275913c6218', '9334 Fremont Street', 1);\r\n";  
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
