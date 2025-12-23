using Note.Domain;
using Note.Domain.Category;

namespace Note.Application.Category
{
    public interface ICategoryServices
    {
        Task<BaseResponse<List<GetCategoryModel>>> Get();
        Task<BaseResponse<GetCategoryModel>> Get(int catgoryId);
        Task<BaseResponse> Insert(AddCategoryModel model);
        Task<BaseResponse> Update(UpdateCategoryModel model);
        Task<BaseResponse> Delete(int categoryId);
    }
}
