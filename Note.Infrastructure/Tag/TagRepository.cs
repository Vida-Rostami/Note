using Dapper;
using Microsoft.Extensions.Options;
using Note.Domain;
using Note.Domain.Tag;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;
namespace Note.Infrastructure.Tag
{

    public class TagRepository : ITagRepository
    {
        private DatabaseSettings _option;
        public TagRepository(IOptions<DatabaseSettings> options)
        {
            _option = options.Value;
        }

        public async Task<BaseResponse<List<GetTagModel>>> GetTag()
        {
            try
            {
                using var connection = new OracleConnection(_option.OracleConnection);
                await connection.OpenAsync();

                using var command = new OracleCommand("Sp_GetAllTag", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("p_Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();

                var tags = new List<GetTagModel>();
                while (await reader.ReadAsync())
                {
                    var tag = new GetTagModel
                    {
                        TagId = reader.GetInt32(reader.GetOrdinal("TagId")),
                        TagName = reader.GetString("TagName"),
                        CreateDateTime = reader.IsDBNull(reader.GetOrdinal("CreateDateTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        ModifyDateTime = reader.IsDBNull(reader.GetOrdinal("ModifyDateTime")) ? null : (reader.GetDateTime(reader.GetOrdinal("ModifyDateTime"))),
                    };
                    tags.Add(tag);
                }

                if (!tags.Any())
                {
                    return new BaseResponse<List<GetTagModel>>
                    {
                        IsSuccess = false,
                        Message = "اطلاعاتی یافت نگردید",
                        Code = 204
                    };
                }


                return new BaseResponse<List<GetTagModel>>
                {
                    IsSuccess = true,
                    Data = tags,
                    Message = "اطلاعات با موفقیت دریافت گردید.",
                    Code = 200
                };
            }
            catch (Exception ex)
            {
                //return new BaseResponse<List<GetTagModel>>
                //{
                //    IsSuccess = false,
                //    Message = $"خطا در دریافت اطلاعات",
                //    Code = 500
                //};
                throw;
            }
        }

        public async Task<BaseResponse<GetTagModel>> GetTag(int tagId)
        {
            try
            {
                using var connection = new OracleConnection(_option.OracleConnection);
                await connection.OpenAsync();

                using var command = new OracleCommand("SP_GetTagById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("P_TagId", OracleDbType.Int32).Value = tagId;
                command.Parameters.Add("P_Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();

                GetTagModel tag = null;
                if (await reader.ReadAsync())
                {
                    tag = new GetTagModel
                    {
                        TagName = reader["TagName"].ToString()
                    };
                }
                if (tag == null)
                {
                    return new BaseResponse<GetTagModel>
                    {
                        IsSuccess = false,
                        Message = "اطلاعاتی یافت نشد",
                        Code = 204
                    };
                }
                return new BaseResponse<GetTagModel>
                {
                    Data = tag,
                    IsSuccess = true,
                    Code = 200,
                    Message = "با موفقیت دریافت شد"
                };
            }
            catch (Exception ex)
            {
                // TODO: log ex.Message
                //return new BaseResponse<GetTagModel>
                //{
                //    IsSuccess = false,
                //    Code = 500,
                //    Message = "خطایی رخ داده است"
                //};
                throw;
            }
        }

        public async Task<BaseResponse> InsertTag(AddTagModel model)
        {
            try
            {
                using (var connection = new OracleConnection(_option.OracleConnection))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("P_TagName", model.TagName, DbType.String, ParameterDirection.Input);
                    await connection.ExecuteAsync("SP_InsertTag", parameters, commandType: CommandType.StoredProcedure);
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Code = 201,
                        Message = "با موفقیت درج شد",
                    };
                }
            }
            catch (Exception ex)
            {
                //log
                //return new BaseResponse
                //{
                //    IsSuccess = false,
                //    Code = 500,
                //    Message = "خطایی رخ داده است"
                //};
                throw;
            }

        }

        public async Task<BaseResponse> UpdateTag(UpdateTagModel model)
        {
            try
            {
                var getTag = GetTag(model.TagId);
                if (getTag != null)
                {
                    if (getTag.Result.Code == 204)
                    {
                        return new BaseResponse
                        {
                            IsSuccess = false,
                            Code = 204,
                            Message = "با شناسه وارد شده داده ای یافت نشد."
                        };
                    }
                }
                using (var connection = new OracleConnection(_option.OracleConnection))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("P_TagId", model.TagId, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("P_TagName", model.TagName, DbType.String, ParameterDirection.Input);
                    await connection.ExecuteAsync("SP_UpdateTag", parameters, commandType: CommandType.StoredProcedure);
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Code = 200,
                        Message = "با موفقیت اپدیت شد",
                    };
                }
            }
            catch (Exception ex)
            {
                //log
                //return new BaseResponse
                //{
                //    IsSuccess = false,
                //    Code = 500,
                //    Message = "خطایی رخ داده است"
                //};
                throw;
            }
        }
        public async Task<BaseResponse> DeleteTag(int tagId)
        {
            try
            {
                var getTag = GetTag(tagId);
                if (getTag != null)
                {
                    if (getTag.Result.Code == 204)
                    {
                        return new BaseResponse
                        {
                            IsSuccess = false,
                            Code = 204,
                            Message = "با شناسه وارد شده داده ای یافت نشد."
                        };
                    }
                }
                using (var connection = new OracleConnection(_option.OracleConnection))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("P_TagId", tagId, DbType.Int32, ParameterDirection.Input);
                    await connection.ExecuteAsync("SP_DeleteTag", parameters, commandType: CommandType.StoredProcedure);
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Code = 200,
                        Message = "با موفقیت حذف شد",
                    };
                }
            }
            catch (Exception ex)
            {
                //log
                //return new BaseResponse
                //{
                //    IsSuccess = false,
                //    Code = 500,
                //    Message = "خطایی رخ داده است"
                //};
                throw;
            }
        }
    }
}
