namespace Shared.DTOs.CRM
{
    public class ContactDto : BaseWithAttrDto
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int CustomerId { get; set; }
    }
}
