namespace Inventra.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public string FirmName { get; set; }
        public string ContactInfo { get; set; }
        public string AuthorizedPerson { get; set; }
    }
}