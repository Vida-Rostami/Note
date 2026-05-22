using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Note.Application.Services.Category;
using Note.Domain;
using Note.Domain.Category;
using Note.Domain.Pagination;

namespace Note.Api.Controllers
{
    [EnableRateLimiting("fixed")]
    [Route("api/v1/[controller]/[action]")]
     [ApiController]
    public class CategoryController
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        //add update by http method

        [HttpGet]
        public async Task<PaginationBaseResposne<List<GetCategoryModel>>> Get(int pageNumber = 1, int pageSize = 10)
        {
            return await _categoryServices.Get(pageNumber, pageSize);
        }

        [HttpGet("id")]
        public async Task<BaseResponse<GetCategoryModel>> Get([FromQuery] int catgoryId)
        {
            return await _categoryServices.Get(catgoryId);
        }

        [HttpPost]
        public Task<BaseResponse> Insert(AddCategoryModel model)
        {
            return _categoryServices.Insert(model);
        }

        [HttpPut]
        public Task<BaseResponse> Update(UpdateCategoryModel model)
        {
            return _categoryServices.Update(model);
        }

        [HttpDelete]
        public async Task<BaseResponse> Delete(int categoryId)
        {
            return await (_categoryServices.Delete(categoryId));
        }
    }
}
