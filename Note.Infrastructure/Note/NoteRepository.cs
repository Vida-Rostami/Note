using Microsoft.Extensions.Options;
using Note.Domain.Common;
using Note.Domain.Models.Note;
using Note.Domain.Response;
using Note.Domain.Settings;
using Note.Infrastructure.Common.Retry;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Polly;
using System.Data;
using System.Net;

namespace Note.Infrastructure.Note
{
    public class NoteRepository : INoteRepository
    {
        private DatabaseSettings _databaseOptions;
        private PaginationSettings _paginationOptions;
        private readonly IAsyncPolicy _policy;
        public NoteRepository(IOptions<DatabaseSettings> options, IOptions<PaginationSettings> paginationOptions)
        {
            _databaseOptions = options.Value;
            _paginationOptions = paginationOptions.Value;
            _policy = PollyHelper.CreateOracleRetryPolicy();
        }

        public async Task<PaginationBaseResposne<List<GetNoteModelResponse>>> GetNote(NoteFilterModel model)
        {
            var pageNumber = model.PageNumber <= 0 ? _paginationOptions.DefaultPageNumber : model.PageNumber;
            var pageSize = model.PageSize <= 0 ? _paginationOptions.DefaultPageSize : Math.Min(model.PageSize, _paginationOptions.MaxPageSize);

            return await _policy.ExecuteAsync(async () =>
            {
                using var connection = new OracleConnection(_databaseOptions.OracleConnection);
                await connection.OpenAsync();
                using var command = new OracleCommand("Sp_GetAllNote", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("p_Title", OracleDbType.NVarchar2).Value = string.IsNullOrWhiteSpace(model.Title) ? (object)DBNull.Value : model.Title;
                command.Parameters.Add("p_Text", OracleDbType.NVarchar2).Value = string.IsNullOrWhiteSpace(model.Text) ? (object)DBNull.Value : model.Text;
                command.Parameters.Add("p_CategoryId", OracleDbType.Int32).Value = model.CategoryId.HasValue ? (object)model.CategoryId.Value : DBNull.Value;
                command.Parameters.Add("p_TagId", OracleDbType.Int32).Value = model.TagId.HasValue ? (object)model.TagId.Value : DBNull.Value;
                command.Parameters.Add("p_FromDate", OracleDbType.Date).Value = string.IsNullOrWhiteSpace(model.PersianFromDate) ? (object)DBNull.Value : (object)ConvertDate.ConvertPersianToGregorian(model.PersianFromDate);
                command.Parameters.Add("p_ToDate", OracleDbType.Date).Value = string.IsNullOrWhiteSpace(model.PersianToDate) ? (object)DBNull.Value : (object)ConvertDate.ConvertPersianToGregorian(model.PersianToDate);
                command.Parameters.Add("p_PageNumber", OracleDbType.Int32).Value = pageNumber;
                command.Parameters.Add("p_PageSize", OracleDbType.Int32).Value = pageSize;
                command.Parameters.Add("p_TotalCount", OracleDbType.Int32).Direction = ParameterDirection.Output;
                command.Parameters.Add("p_Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();

                var notesDictionary = new Dictionary<int, GetNoteModelResponse>();

                while (await reader.ReadAsync())
                {
                    int noteId = reader.GetInt32(reader.GetOrdinal("NoteId"));

                    if (!notesDictionary.TryGetValue(noteId, out var note))
                    {
                        note = new GetNoteModelResponse
                        {
                            NoteId = noteId,
                            Title = reader["Title"]?.ToString(),
                            Text = reader["Text"]?.ToString(),
                            CategoryName = reader["CategoryName"]?.ToString(),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            Tag = new List<int>(),
                            TagName = new List<string>(),
                            CreateDateTime = reader.IsDBNull(reader.GetOrdinal("CreateDateTime")) ? null : reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ModifyDateTime = reader.IsDBNull(reader.GetOrdinal("ModifyDateTime")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifyDateTime")),
                            IsEditable = reader.GetInt32(reader.GetOrdinal("IsEditable"))
                        };

                        notesDictionary.Add(noteId, note);
                    }
                    var tagName = reader["TagName"]?.ToString();
                    if (!string.IsNullOrEmpty(tagName) && !note.TagName.Contains(tagName))
                    {
                        note.TagName.Add(tagName);
                    }
                }

                var totalObj = command.Parameters["p_TotalCount"].Value;

                var totalCount = totalObj == null || totalObj == DBNull.Value
                    ? 0
                    : ((OracleDecimal)totalObj).ToInt32();

                var notes = notesDictionary.Values.ToList();

                return new PaginationBaseResposne<List<GetNoteModelResponse>>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    IsSuccess = notes.Any(),
                    Data = notes,
                    Code = notes.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NoContent,
                    Message = notes.Any() ? "اطلاعات با موفقیت دریافت گردید." : "اطلاعاتی یافت نشد"
                };
            });
        }
        public async Task<BaseResponse<GetNoteModelResponse>> GetNote(int noteId)
        {
            return await _policy.ExecuteAsync(async () =>
            {
                using var connection = new OracleConnection(_databaseOptions.OracleConnection);
                await connection.OpenAsync();

                using var command = new OracleCommand("Sp_GetNoteById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("p_noteId", OracleDbType.Int32).Value = noteId;
                command.Parameters.Add("p_Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                using var reader = await command.ExecuteReaderAsync();

                GetNoteModelResponse note = null;

                while (await reader.ReadAsync())
                {
                    if (note == null)
                    {
                        note = new GetNoteModelResponse
                        {
                            NoteId = reader.GetInt32(reader.GetOrdinal("NoteId")),
                            Title = reader["Title"]?.ToString(),
                            Text = reader["Text"]?.ToString(),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            CategoryName = reader["CATEGORYNAME"]?.ToString(),
                            Tag = new List<int>(),
                            TagName = new List<string>(),
                            CreateDateTime = reader.IsDBNull(reader.GetOrdinal("CreateDateTime")) ? null : reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ModifyDateTime = null,
                            IsEditable = reader.GetInt32(reader.GetOrdinal("IsEditable"))
                        };
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("TAGID")))
                        note.Tag.Add(reader.GetInt32(reader.GetOrdinal("TAGID")));

                    if (!reader.IsDBNull(reader.GetOrdinal("TAGNAME")))
                        note.TagName.Add(reader["TAGNAME"].ToString());
                }

                if (note == null)
                {
                    return new BaseResponse<GetNoteModelResponse>
                    {
                        IsSuccess = false,
                        Message = "اطلاعاتی یافت نشد",
                        Code = (int)HttpStatusCode.NoContent
                    };
                }

                return new BaseResponse<GetNoteModelResponse>
                {
                    IsSuccess = true,
                    Data = note,
                    Code = (int)HttpStatusCode.OK,
                    Message = "با موفقیت دریافت شد"
                };
            });
        }

        public async Task<BaseResponse> InsertNote(AddNoteModel model)
        {
            using var connection = new OracleConnection(_databaseOptions.OracleConnection);
            await connection.OpenAsync();

            using var command = new OracleCommand("SP_InsertNoteString", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("p_Title", OracleDbType.NVarchar2).Value = model.Title;
            command.Parameters.Add("p_Text", OracleDbType.NVarchar2).Value = model.Text;
            command.Parameters.Add("p_CategoryId", OracleDbType.Int32).Value = model.CategoryId;
            command.Parameters.Add("p_IsEditable", OracleDbType.Int32).Value = model.IsEditable;

            var tagsString = model.Tags != null ? string.Join(",", model.Tags) : null;
            command.Parameters.Add("p_Tags", OracleDbType.NVarchar2).Value = tagsString;

            await command.ExecuteNonQueryAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = " با موفقیت ثبت شد",
                Code = (int)HttpStatusCode.Created
            };
        }

        public async Task<BaseResponse> UpdateNote(UpdateNoteModel model)
        {
            var getNote = await GetNote(model.NoteId);
            if (getNote != null)
            {
                if (getNote.Code == (int)HttpStatusCode.NoContent)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Code = (int)HttpStatusCode.NoContent,
                        Message = "با شناسه وارد شده داده ای یافت نشد."
                    };
                }
            }
            using var connection = new OracleConnection(_databaseOptions.OracleConnection);
            await connection.OpenAsync();

            using var command = new OracleCommand("SP_UpdateNote", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("p_NoteId", OracleDbType.Int32).Value = model.NoteId;
            command.Parameters.Add("p_Title", OracleDbType.NVarchar2).Value = model.Title;
            command.Parameters.Add("p_Text", OracleDbType.NVarchar2).Value = model.Text;
            command.Parameters.Add("p_CategoryId", OracleDbType.Int32).Value = model.CategoryId;

            var tagsString = model.Tags != null ? string.Join(",", model.Tags) : null;
            command.Parameters.Add("p_Tags", OracleDbType.NVarchar2).Value = tagsString;

            await command.ExecuteNonQueryAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = " با موفقیت بروزرسانی شد",
                Code = (int)HttpStatusCode.OK
            };
        }

        public async Task<BaseResponse> DeleteNote(int noteId)
        {
            var getNote = await GetNote(noteId);
            if (getNote != null)
            {
                if (getNote.Code == (int)HttpStatusCode.NoContent)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Code = (int)HttpStatusCode.NoContent,
                        Message = "با شناسه وارد شده داده ای یافت نشد."
                    };
                }
            }
            using var connection = new OracleConnection(_databaseOptions.OracleConnection);
            await connection.OpenAsync();

            using var command = new OracleCommand("SP_DeleteNote", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("p_NoteId", OracleDbType.Int32).Value = noteId;

            await command.ExecuteNonQueryAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "با موفقیت حذف شد",
                Code = (int)HttpStatusCode.OK
            };
        }
    }
}