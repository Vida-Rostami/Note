using Note.Domain.Tag;
using Note.Domain;

namespace Note.Application.Services.Tag
{
    public interface ITagServices
    {
        Task<BaseResponse<List<GetTagModel>>> GetTag();
        Task<BaseResponse<GetTagModel>> GetTag(int tagId);
        Task<BaseResponse> InsertTag(AddTagModel model);
        Task<BaseResponse> UpdateTag(UpdateTagModel model);
        Task<BaseResponse> DeleteTag(int tagId);
    }
}
