using Microsoft.AspNetCore.Mvc;
using Note.Application.Category;
using Note.Model;
using Note.Model.Category;
using System.ComponentModel.DataAnnotations;

namespace Note.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            throw new ValidationException("TagName is required");

            _categoryServices = categoryServices;
        }

        //add update by http method

        [HttpGet]
        public async Task<BaseResponse<List<GetCategoryModel>>> Get()
        {
            return await _categoryServices.Get();
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
