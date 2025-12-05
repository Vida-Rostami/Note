using Note.Model.Tag;
using Note.Model;

namespace Note.Application.Tag
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
