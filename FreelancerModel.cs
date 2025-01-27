using System.Collections.Generic;
namespace freelancers
{
    public class Freelancer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsArchived { get; set; } = false;

        // One-to-Many Relationships
        public List<Skillset> Skillsets { get; set; } = new List<Skillset>();
        public List<Hobby> Hobbies { get; set; } = new List<Hobby>();
    }

    public class Skillset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FreelancerId { get; set; }
    }

    public class Hobby
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FreelancerId { get; set; }
    }
}
