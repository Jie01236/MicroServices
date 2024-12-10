using System.ComponentModel;

namespace MessagingBus.MessagingModels
{
    public class Reservation
    {
        public int Id { get; set; } 
        public int UserId { get; set; } 
        public int PropertyId { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public int Guest {  get; set; }
        public float Price { get; set; }

        [DefaultValue("Confirmed")]
        public string Status { get; set; }
    }
}
