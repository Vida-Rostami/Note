using Note.Model.Common;

namespace Note.Model.Tag
{
    public class GetTagModel
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string ShamsiCreateDateTime => CreateDateTime.ToShamsi();
        public DateTime? ModifyDateTime { get; set; }
        public string ShamsiModifyDateTime => ModifyDateTime.ToShamsi();
    }
}
