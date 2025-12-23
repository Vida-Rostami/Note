using Note.Model.Common;

namespace Note.Model.Category
{
    public class GetCategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string ShamsiCreateDateTime => CreateDateTime.ToShamsi();
        public DateTime? ModifyDateTime { get; set; }
        public string ShamsiModifyDateTime => ModifyDateTime.ToShamsi();
    }
}
