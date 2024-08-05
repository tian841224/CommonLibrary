using CommonLibrary.DTOs;
using Microsoft.AspNetCore.Http;

namespace CommonLibrary.Interface
{
    public interface IFileService
    {
        Task<FileUploadDto> UploadFile(string FileName, IFormFile FormFile, string? FileUploadPath = null);
        Task<string> FileToBase64(IFormFile formFile);
        Task<string> FileToBase64(string filePath);
        Task<byte[]> FileToByte(string filePath);
        FileStream DownloadFile(string filePath);
        string GetFileUploadPath() ;
    }
}
