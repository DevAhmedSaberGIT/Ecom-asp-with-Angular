using Ecom.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace Ecom.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromMinutes(value:30);

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env, IMemoryCache memoryCache)
        {
            _next = next;
            _env = env;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // تطبيق ترويسات الأمان في كل الطلبات
                ApplySecurity(context);

                if (IsRequestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";

                    var response = new ApiExceptions(
                        statusCode: (int)HttpStatusCode.TooManyRequests,
                        message: "Too many requests. Please try again later"
                    );

                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                // تطبيق ترويسات الأمان حتى في حالة الخطأ
                ApplySecurity(context);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsEnvironment("Development")
                    ? new ApiExceptions(
                        statusCode: (int)HttpStatusCode.InternalServerError,
                        message: ex.Message,
                        details: ex.StackTrace)
                    : new ApiExceptions(
                        statusCode: (int)HttpStatusCode.InternalServerError,
                        message: "Internal Server Error");

                string json = System.Text.Json.JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }

        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            var cacheKey = $"Rate:{ip}";
            var dateNow = DateTime.Now;

            var (timesStamp, count) = _memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (dateNow, 0);
            });

            if (dateNow - timesStamp < _rateLimitWindow)
            {
                if (count >= 8)
                {
                    return false;
                }

                _memoryCache.Set(cacheKey, (timesStamp, count + 1), absoluteExpirationRelativeToNow: _rateLimitWindow);
            }
            else
            {
                _memoryCache.Set(cacheKey, (dateNow, 1), absoluteExpirationRelativeToNow: _rateLimitWindow);
            }

            return true;
        }

        private void ApplySecurity(HttpContext context)
        {
            // تمنع المتصفح من محاولة استنتاج نوع الملف إذا لم يتم تحديده (حماية من بعض أنواع الهجمات)
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";

            // تفعيل حماية XSS في المتصفحات القديمة (زي Internet Explorer)
            context.Response.Headers["X-XSS-Protection"] = "1;mode=block";

            // تمنع عرض الصفحة داخل iframe لحمايتها من هجمات Clickjacking
            context.Response.Headers["X-Frame-Options"] = "DENY";

            // سياسة لتحديد مصادر تحميل المحتوى لحماية من XSS والـ Data Injection
            context.Response.Headers["Content-Security-Policy"] =
                "default-src 'self'; script-src 'self'; object-src 'none'; frame-ancestors 'none';";

            // تمنع إرسال ترويسة Referer مع الطلبات (لتقليل تسريب معلومات حساسة)
            context.Response.Headers["Referrer-Policy"] = "no-referrer";

            // تمنع الوصول إلى مكونات الجهاز مثل الكاميرا والمايكروفون والموقع الجغرافي
            context.Response.Headers["Permissions-Policy"] =
                "camera=(), microphone=(), geolocation=(), fullscreen=(), payment=()";

            // تجبر المتصفح على استخدام HTTPS فقط وتمنع التحويل التلقائي لـ HTTP
            context.Response.Headers["Strict-Transport-Security"] =
                "max-age=63072000; includeSubDomains; preload";

            // تمنع التلاعب في المحتوى من خلال البروكسيات أو الشبكات الوسيطة
            context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, proxy-revalidate";

            // تمنع استرجاع الصفحة من الكاش
            context.Response.Headers["Pragma"] = "no-cache";

            // ترويسة لإعلام البروكسيات والسيرفرات بعدم تخزين الاستجابة
            context.Response.Headers["Expires"] = "0";

            // تمنع مشاركة الكوكيز مع دومينات أخرى (حماية من CSRF)
            context.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin";

            // تمنع الصفحة من الوصول لمحتوى من مصادر خارجية إلا لو كان مسموح صراحة
            context.Response.Headers["Cross-Origin-Embedder-Policy"] = "require-corp";

            // تمنع تحميل أو تنفيذ ملفات من cross-origin sources بدون إذن
            context.Response.Headers["Cross-Origin-Resource-Policy"] = "same-origin";
        }

    }
}
