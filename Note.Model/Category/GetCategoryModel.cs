using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Model.Category
{
    public class GetCategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
    }
}
