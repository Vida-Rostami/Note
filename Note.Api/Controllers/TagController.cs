using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Note.Application.Tag;
using Note.Model;
using Note.Model.Tag;

namespace Note.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ITagServices
    {
        public TagController()
        {
            
        }
        [HttpGet]
        public Task<BaseResponse<GetTagModel>> GetTag()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<BaseResponse<GetTagModel>> GetTag(int catgtagIdoryId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<BaseResponse> InsertTag(AddTagModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<BaseResponse> UpdateTag(UpdateTagModel model)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public Task<BaseResponse> DeleteTag(int TagId)
        {
            throw new NotImplementedException();
        }
    }
}
