using BlogCMS.Interfaces;
namespace BlogCMS.Models
{
    public class Post: IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Published { get; set; }
    }
}
