using System;

namespace HumanResourcesDBFirst.Models
{
    public class WishSuggestion
    {
        public int Id { get; set; }
        
        public int? EmployeeId { get; set; }
        
        public string Title { get; set; } = null!;
        
        public string Content { get; set; } = null!;
        
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        
        public string Status { get; set; } = "Beklemede";
        
        public string? AdminReply { get; set; }
        
        public virtual Employee? Employee { get; set; }
    }
}
