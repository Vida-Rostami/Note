using Microsoft.Extensions.Options;
using Note.Application.Commons;
using Note.Domain;
using Note.Domain.Category;
using Note.Domain.Pagination;
using Note.Infrastructure.Caching;
using Note.Infrastructure.Category;
using System.Net;
using System.Reflection;

namespace Note.Application.Services.Category
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICacheService _cacheService;
        private PaginationSettings _paginationOptions;

        public CategoryServices(ICategoryRepository categoryRepository, ICacheService cacheService, IOptions<PaginationSettings> paginationOptions)
        {
            _categoryRepository = categoryRepository;
            _cacheService = cacheService;
            _paginationOptions = paginationOptions.Value;
        }

        public async Task<PaginationBaseResposne<List<GetCategoryModel>>> Get(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? _paginationOptions.DefaultPageNumber : pageNumber;
            pageSize = pageSize <= 0 ? _paginationOptions.DefaultPageSize : Math.Min(pageSize, _paginationOptions.MaxPageSize);
            var version = await _cacheService.GetVersion(CacheKeys.CategoryVersion);

            var cacheKey = GenerateListCacheKey(version, pageNumber, pageSize);

            var cachedData =
                await _cacheService.Get<PaginationBaseResposne<List<GetCategoryModel>>>(cacheKey);

            if (cachedData != null)
                return cachedData;

            var data =
                await _categoryRepository.Get(pageNumber, pageSize);

            if (data == null)
            {
                return new PaginationBaseResposne<List<GetCategoryModel>>
                {
                    Code = (int)HttpStatusCode.NoContent,
                    IsSuccess = false,
                    Message = "اطلاعاتی یافت نشد."
                };
            }

            await _cacheService.Set(
                cacheKey,
                data,
                TimeSpan.FromMinutes(5));

            return data;
        }

        public async Task<BaseResponse<GetCategoryModel>> Get(int categoryId)
        {
            var version =
                await _cacheService.GetVersion(CacheKeys.CategoryVersion);

            var cacheKey =
                GenerateDetailCacheKey(version, categoryId);

            var cachedCategory =
                await _cacheService.Get<BaseResponse<GetCategoryModel>>(cacheKey);

            if (cachedCategory != null)
                return cachedCategory;

            var data =
                await _categoryRepository.Get(categoryId);

            if (data == null)
            {
                return new BaseResponse<GetCategoryModel>
                {
                    Code = (int)HttpStatusCode.NoContent,
                    IsSuccess = false,
                    Message = "اطلاعاتی یافت نشد."
                };
            }

            await _cacheService.Set(
                cacheKey,
                data,
                TimeSpan.FromMinutes(5));

            return data;
        }

        public async Task<BaseResponse> Insert(AddCategoryModel model)
        {
            var result =
                await _categoryRepository.Insert(model);

            if (result.IsSuccess)
            {
                await _cacheService.IncrementVersion(CacheKeys.CategoryVersion);
            }

            return result;
        }


        public async Task<BaseResponse> Update(UpdateCategoryModel model)
        {
            var result =
                await _categoryRepository.Update(model);

            if (result.IsSuccess)
            {
                await _cacheService.IncrementVersion(CacheKeys.CategoryVersion);
            }

            return result;
        }


        public async Task<BaseResponse> Delete(int categoryId)
        {
            var result =
                await _categoryRepository.Delete(categoryId);

            if (result.IsSuccess)
            {
                await _cacheService.IncrementVersion(CacheKeys.CategoryVersion);
            }

            return result;
        }
        private string GenerateListCacheKey(
            int version,
            int pageNumber,
            int pageSize)
        {
            return $"category_v{version}_list_{pageNumber}_{pageSize}";
        }


        private string GenerateDetailCacheKey(
            int version,
            int categoryId)
        {
            return $"category_v{version}_detail_{categoryId}";
        }
    }
}