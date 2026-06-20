using Note.Domain.Pagination;

namespace Note.Domain.Models.Note
{
    public class NoteFilterModel : PaginationParams
    {
        public string? Title { get; set; }
        public string? Text { get; set; }
        public int? CategoryId { get; set; }
        public int? TagId { get; set; }
        public string? PersianFromDate { get; set; }
        public string? PersianToDate { get; set; }
    }
}
