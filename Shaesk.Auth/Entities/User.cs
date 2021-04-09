using Shaesk.Auth.Entities.Common;

namespace Shaesk.Auth.Entities
{
    /// <summary>
    /// <para>EN:If there is a company or a group, the purpose of ParentId is to connect the user with it and generate tokens.</para> 
    /// <para>TR:Bir şirket veya grup varsa, ParentId'nin amacı kullanıcıyı ona bağlamak ve token oluşturmaktır.</para> 
    /// </summary>
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public int ParentId { get; set; } = 0;
        public bool IsEmailVerification { get; set; }
        public bool IsSmsVerification { get; set; }
        public string Role { get; set; }
    }
}
