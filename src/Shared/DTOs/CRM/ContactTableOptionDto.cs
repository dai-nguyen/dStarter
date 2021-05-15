namespace Shared.DTOs.CRM
{
    public class ContactTableOptionDto : TableOptionDto
    {        
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CustomerId { get; set; }
    }
}
