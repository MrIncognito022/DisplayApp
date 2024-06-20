using System.ComponentModel.DataAnnotations;

namespace DisplayApp.Models.Domain
{
    public class Comments
    {
        [Key]
        public int CommentsId { get; set; }
        public string UniqueId { get; set; }
        public string? Comment { get; set; }
        public string? Tags { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
