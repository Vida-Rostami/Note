using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Model
{
    public static class ErrorMessages
    {
        public const string InternalServerError = "خطای داخلی سرور رخ داده است. لطفاً بعداً تلاش کنید.";

        public const string NotFound = "صفحه‌ای با این آدرس پیدا نشد.";

        public const string BadRequest = "درخواست ارسال‌شده نامعتبر است.";
    }
}
