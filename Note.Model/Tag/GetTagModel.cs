using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Note.Model.Tag
{
    public class GetTagModel
    {
        public int TagId { get; set; }
        public  string TagName { get; set; }
        public DateTime? CreateDateTime { get; set; }    
        public DateTime? ModifyDateTime { get; set; }
    }
}
