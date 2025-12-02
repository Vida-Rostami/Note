using Note.Model;
using Note.Model.Category;

namespace Note.Infrastructure.Category
{


    public class CategoryRepository : ICategoryRepository
    {
        private readonly IOptions<DatabaseSettings> _options;

        public CategoryRepository(IOptions<DatabaseSettings> options)
        {
            _options = options;
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
