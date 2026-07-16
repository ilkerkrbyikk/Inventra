namespace Inventra.Domain.Entities
{
    public abstract class BaseEntity
    {
        //ID Tipleri Guid Olarak Tut
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}