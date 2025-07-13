namespace RDDShop.Models
{
    public enum Status
    {
        Active=1,
        Inactive=2,
    }
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       
        public Status Status { get; set; } = Status.Active;

    }
}
