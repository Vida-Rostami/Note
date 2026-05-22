using Note.Infrastructure.Category;
using Note.Domain;
using Note.Domain.Category;
using Note.Infrastructure.Caching;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Note.Domain.Pagination;

namespace Note.Application.Services.Category
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

            public async Task<PaginationBaseResposne<List<GetCategoryModel>>> Get(int pageNumber, int pageSize)
            {
                var cacheKey = $"category_{pageNumber}_{pageSize}";
                var cachedCategory = await _cacheService.Get<List<GetCategoryModel>>(cacheKey);
                if (cachedCategory != null)
                {
                    return new PaginationBaseResposne<List<GetCategoryModel>>
                    {
                        Data = cachedCategory,
                        IsSuccess = true,
                        Code = 200,
                        Message = "با موفقیت دریافت گردید."
                    };
                }


                var data = await _categoryRepository.Get(pageNumber, pageSize);
                if (data == null)
                {
                    return new PaginationBaseResposne<List<GetCategoryModel>>
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
            await _cacheService.Remove("category_all");
            return await _categoryRepository.Insert(model);
        }

        public async Task<BaseResponse> Update(UpdateCategoryModel model)
        {
            await _cacheService.Remove("category_all");
            return await _categoryRepository.Update(model);
        }

        public async Task<BaseResponse> Delete(int categoryId)
        {
            await _cacheService.Remove("category_all");
            return await _categoryRepository.Delete(categoryId);
        }
    }
}
