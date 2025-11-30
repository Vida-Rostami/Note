using Microsoft.AspNetCore.Mvc;
using Note.Application.Category;
using Note.Model;
using Note.Model.Category;

namespace Note.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ICategoryServices
    {
        public CategoryController()
        {

        }

        //add update by http method

        [HttpGet]
        public Task<BaseResponse<GetCategoryModel>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("id")]
        public Task<BaseResponse<GetCategoryModel>> Get([FromRoute]int catgoryId)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public Task<BaseResponse> Insert(AddCategoryModel model)
        {
            throw new NotImplementedException();
        }
        [HttpPut]
        public Task<BaseResponse> Update(UpdateCategoryModel model)
        {
            throw new NotImplementedException();
        }
        [HttpDelete]
        public Task<BaseResponse> Delete(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
