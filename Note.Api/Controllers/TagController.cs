using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Note.Application.Tag;
using Note.Model;
using Note.Model.Tag;

namespace Note.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController
    {
        private readonly ITagServices _tagServices;

        public TagController(ITagServices tagServices)
        {
            _tagServices = tagServices;
        }
        [HttpGet]
        public async Task<BaseResponse<List<GetTagModel>>> GetTag()
        {
            return await _tagServices.GetTag();
        }

        [HttpGet("tagId")]
        public async Task<BaseResponse<GetTagModel>> GetTag(int tagId)
        {
            return await _tagServices.GetTag(tagId);
        }

        [HttpPost]
        public async Task<BaseResponse> InsertTag(AddTagModel model)
        {
            return await _tagServices.InsertTag(model);
        }

        [HttpPut]
        public async Task<BaseResponse> UpdateTag(UpdateTagModel model)
        {
            return await _tagServices.UpdateTag(model);
        }

        [HttpDelete]
        public async Task<BaseResponse> DeleteTag(int tagId)
        {
            return await _tagServices.DeleteTag(tagId);
        }
    }
}
