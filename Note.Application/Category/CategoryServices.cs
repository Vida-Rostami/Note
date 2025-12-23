using Note.Infrastructure.Category;
using Note.Domain;
using Note.Domain.Category;

namespace Note.Application.Category
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryServices(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<BaseResponse<List<GetCategoryModel>>> Get()
        {
            return await _categoryRepository.Get();
        }

        public async Task<BaseResponse<GetCategoryModel>> Get(int catgoryId)
        {
            return await _categoryRepository.Get(catgoryId);
        }

        public async Task<BaseResponse> Insert(AddCategoryModel model)
        {
            return await _categoryRepository.Insert(model);
        }

        public async Task<BaseResponse> Update(UpdateCategoryModel model)
        {
            return await _categoryRepository.Update(model);
        }

        public async Task<BaseResponse> Delete(int categoryId)
        {
            return await _categoryRepository.Delete(categoryId);
        }
    }
}
