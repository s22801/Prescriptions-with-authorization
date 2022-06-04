using System;

namespace APBD_8.Entities
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; internal set; }
        public string Email { get; internal set; }
        public string RefreshToken { get; internal set; }
        public DateTime TokenExpirationTime { get; internal set; }
        public string Salt { get; internal set; }
    }
}
