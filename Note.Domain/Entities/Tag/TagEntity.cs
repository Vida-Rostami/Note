namespace Note.Domain.Entities.Tag
{
    public class TagEntity
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? ModifyDateTime { get; set; }
    }
}
