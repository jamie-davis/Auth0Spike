using System.Collections.Generic;

namespace Auth0Con.Services
{
    internal interface IUserOperations
    {
        IEnumerable<RegisteredUser> GetAllUsers();
        RegisteredUser AddUser(string email, string password);
        void DeleteUser(string id);
    }
}