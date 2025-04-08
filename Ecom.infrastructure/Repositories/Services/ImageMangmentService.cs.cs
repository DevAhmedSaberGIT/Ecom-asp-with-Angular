using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories.Services
{
    public class ImageMangmentService : IImageMangmentService
    {
        private readonly IFileProvider fileProvider;

        public ImageMangmentService(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }

        public async Task<List<string>> AddImageAsyns(IFormFileCollection files, string src)
        {
            // قائمة لتخزين أسماء الصور المحفوظة أو المسارات النسبية
            List<string> savedImages = new List<string>();

            // بناء مسار المجلد الذي ستُحفظ فيه الصور
            string imageDirectory = Path.Combine("wwwroot", "Image", src);

            // التأكد من وجود المجلد، وإن لم يكن موجوداً يتم إنشاؤه
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            // المرور على كل ملف تم استلامه
            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    // إنشاء اسم ملف فريد باستخدام Guid مع الحفاظ على امتداد الملف الأصلي
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string fullPath = Path.Combine(imageDirectory, fileName);

                    // حفظ الملف على القرص
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // إضافة المسار النسبي للملف إلى القائمة (يمكن استخدامه لاحقاً لاسترجاع الصورة)
                    savedImages.Add(Path.Combine(src, fileName));
                }
            }

            return savedImages;
        }

     
        public async Task<string> DelteImageAsyns(string src)
        {
            var fileInfo = fileProvider.GetFileInfo(src);
            var physicalPath = fileInfo.PhysicalPath;

            if (string.IsNullOrEmpty(physicalPath) || !File.Exists(physicalPath))
            {
                return await Task.FromResult("Image not found.");
            }

            try
            {
                File.Delete(physicalPath);
                return await Task.FromResult("Image deleted successfully.");
            }
            catch (Exception ex)
            {
                // يمكنك تعديل الرسالة أو تسجيل الخطأ حسب الحاجة
                return await Task.FromResult($"Error deleting image: {ex.Message}");
            }
        }

    
    }
}
