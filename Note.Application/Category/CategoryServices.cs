using Note.Infrastructure.Category;
using Note.Domain;
using Note.Domain.Category;
using Note.Infrastructure.Caching;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Note.Application.Category
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICacheService _cacheService;

        public CategoryServices(ICategoryRepository categoryRepository, ICacheService cacheService)
        {
            _categoryRepository = categoryRepository;
            _cacheService = cacheService;
        }

        public async Task<BaseResponse<List<GetCategoryModel>>> Get()
        {
            var cacheKey = $"category_all";
            var cachedCategory = await _cacheService.Get<List<GetCategoryModel>>(cacheKey);
            if (cachedCategory != null)
            {
                return new BaseResponse<List<GetCategoryModel>>
                {
                    Data = cachedCategory,
                    IsSuccess = true,
                    Code = 200,
                    Message = "با موفقیت دریافت گردید."
                };
            }


            var data = await _categoryRepository.Get();
            if (data == null)
            {
                return new BaseResponse<List<GetCategoryModel>>
                {
                    Code = 204,
                    IsSuccess = true,
                    Message = "اطلاعاتی یافت نگردید."
                };
            }
            await _cacheService.Set(cacheKey, data.Data, TimeSpan.FromMinutes(5));
            return data;
        }

        public async Task<BaseResponse<GetCategoryModel>> Get(int catgoryId)
        {
            var cacheKey = $"category_{catgoryId}";
            var cachedCategory = await _cacheService.Get<GetCategoryModel>(cacheKey);
            if (cachedCategory != null)
            {
                return new BaseResponse<GetCategoryModel>
                {
                    Data = cachedCategory,
                    IsSuccess = true,
                    Code = 200,
                    Message = "با موفقیت دریافت گردید."
                };
            }


            var data = await _categoryRepository.Get(catgoryId);
            if (data == null)
            {
                return new BaseResponse<GetCategoryModel>
                {
                    Code = 204,
                    IsSuccess = true,
                    Message = "اطلاعاتی یافت نگردید."
                };
            }
            await _cacheService.Set(cacheKey, data.Data, TimeSpan.FromMinutes(5));
            return data;
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
