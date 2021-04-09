namespace Shaesk.Auth.Entities.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}
