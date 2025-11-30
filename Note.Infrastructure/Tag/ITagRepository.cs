using Note.Model;
using Note.Model.Tag;

namespace Note.Infrastructure.Tag
{
    public interface ITagRepository
    {
        Task<BaseResponse<GetTagModel>> GetTag();
        Task<BaseResponse<GetTagModel>> GetTag(int tagId);
        Task<BaseResponse> InsertTag(AddTagModel model); 
        Task<BaseResponse> UpdateTag(UpdateTagModel model);
        Task<BaseResponse> DeleteTag(int TagId);
    }
}
