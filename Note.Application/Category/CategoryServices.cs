using Note.Model;
using Note.Model.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Application.Category
{
    public class CategoryServices : ICategoryServices
    {
        public CategoryServices()
        {
            
        }

        public Task<BaseResponse<GetCategoryModel>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<GetCategoryModel>> Get(int catgoryId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> Insert(AddCategoryModel model)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> Update(UpdateCategoryModel model)
        {
            throw new NotImplementedException();
        }
        public Task<BaseResponse> Delete(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
