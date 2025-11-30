using Note.Model;
using Note.Model.Category;

namespace Note.Infrastructure.Category
{
    public interface ICategoryRepository
    {
        Task<BaseResponse<GetCategoryModel>> Get();
        Task<BaseResponse<GetCategoryModel>> Get(int catgoryId);
        Task<BaseResponse> Insert(AddCategoryModel model);
        Task<BaseResponse> Update(UpdateCategoryModel model);
        Task<BaseResponse> Delete(int categoryId);
    }
}
