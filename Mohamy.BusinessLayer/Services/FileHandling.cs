﻿using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.Entity.Files;
using Mohamy.RepositoryLayer.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Mohamy.BusinessLayer.Services
{
    public class FileHandling : IFileHandling
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUnitOfWork unitOfWork;

        public FileHandling(IWebHostEnvironment _webHostEnvironment, IUnitOfWork _unitOfWork)
        {
            webHostEnvironment = _webHostEnvironment;
            unitOfWork = _unitOfWork;
        }

        #region Photo Handling

        public async Task<string> UploadFile(IFormFile file, Paths paths, string oldFilePath = null)
        {
            var uploads = Path.Combine(webHostEnvironment.WebRootPath, paths.Name);

            // التحقق من وجود المجلد
            if (!Directory.Exists(uploads))
            {
                throw new DirectoryNotFoundException($"The directory does not exist: {uploads}");
            }

            var uniqueFileName = $"{RandomString(10)}_{file.FileName}";
            var filePath = Path.Combine(uploads, uniqueFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var image = new Images
            {
                Name = uniqueFileName,
                pathId = paths.Id,
                path = paths
            };
            await unitOfWork.ImagesRepository.AddAsync(image);
            await unitOfWork.SaveChangesAsync();

            if (!string.IsNullOrEmpty(oldFilePath) && File.Exists(Path.Combine(webHostEnvironment.WebRootPath, oldFilePath)))
            {
                File.Delete(Path.Combine(webHostEnvironment.WebRootPath, oldFilePath));
            }

            return image.Id;
        }

        public async Task<string> DefaultProfile(Paths paths)
        {
            var uploads = Path.Combine(webHostEnvironment.WebRootPath, paths.Name);
            var sourcePath = Path.Combine(webHostEnvironment.WebRootPath, "asset", "user.jpg");

            // التحقق من وجود المجلد
            if (!Directory.Exists(uploads))
            {
                throw new DirectoryNotFoundException($"The directory does not exist: {uploads}");
            }

            var uniqueFileName = $"{RandomString(10)}_UserIcon.jpg";
            var destinationPath = Path.Combine(uploads, uniqueFileName);
            File.Copy(sourcePath, destinationPath, true);

            var image = new Images
            {
                Name = uniqueFileName,
                pathId = paths.Id,
                path = paths
            };
            await unitOfWork.ImagesRepository.AddAsync(image);
            await unitOfWork.SaveChangesAsync();
            return image.Id;
        }

        public async Task<string> UpdateFile(IFormFile file, Paths paths, string imageId)
        {
            var image = await unitOfWork.ImagesRepository
                .FindAsync(a => a.Id == imageId, include: query =>
                query.Include(q => q.path));

            if (image == null)
            {
                throw new ArgumentException("Image not found");
            }

            var uploads = Path.Combine(webHostEnvironment.WebRootPath, paths.Name);

            // التحقق من وجود المجلد
            if (!Directory.Exists(uploads))
            {
                throw new DirectoryNotFoundException($"The directory does not exist: {uploads}");
            }

            var uniqueFileName = $"{RandomString(10)}_{file.FileName}";
            var filePath = Path.Combine(uploads, uniqueFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var oldFilePath = Path.Combine(webHostEnvironment.WebRootPath, $"{image.path.Name}/{image.Name}");
            image.Name = uniqueFileName;
            image.pathId = paths.Id;
            image.path = paths;

            unitOfWork.ImagesRepository.Update(image);
            await unitOfWork.SaveChangesAsync();

            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }

            return image.Id;
        }


        public async Task<string> GetFile(string imageId)
        {
            var image = await unitOfWork.ImagesRepository
                .FindAsync(a => a.Id == imageId, include: query => 
                query.Include(q => q.path));

            if (image == null)
            {
                throw new FileNotFoundException("لم يتم العثور على الصورة");
            }

            return Path.Combine($"/{image.path.Name}/{image.Name}");
        }

        public async Task<Dictionary<string, string>> GetAllFiles(IEnumerable<string> imageIds)
        {
            var images = await unitOfWork.ImagesRepository
                .FindAllAsync(i => imageIds.Contains(i.Id), include: query =>
                query.Include(i => i.path));

            return images.ToDictionary(image => image.Id, image => Path.Combine($"/{image.path.Name}/{image.Name}"));
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion Photo Handling
    }
}
