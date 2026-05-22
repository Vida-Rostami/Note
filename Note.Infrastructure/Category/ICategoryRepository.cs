using Note.Domain;
using Note.Domain.Category;
using Note.Domain.Pagination;

namespace Note.Infrastructure.Category
{
    public interface ICategoryRepository
    {
        Task<PaginationBaseResposne<List<GetCategoryModel>>> Get(int pageNumber , int pageSize);
        Task<BaseResponse<GetCategoryModel>> Get(int catgoryId);
        Task<BaseResponse> Insert(AddCategoryModel model);
        Task<BaseResponse> Update(UpdateCategoryModel model);
        Task<BaseResponse> Delete(int categoryId);
    }
}
