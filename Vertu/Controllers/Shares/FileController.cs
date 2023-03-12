using Meta.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta.Shares
{
    /// <summary>
    /// 文件管理
    /// </summary>
    [Description("文件管理")]
    public class FileController : MetaController
    {
        /// <summary>
        /// 允许的文件类型
        /// </summary>
        [Description("允许的文件类型")]
        private readonly string[] CommitFileExtends = new String[] { ".mp4", ".jpg", ".png", ".pdf", ".xls", ".xlsx" };

        /// <summary>
        /// 允许的二进制图片类型
        /// </summary>
        [Description("允许的二进制图片类型")]
        private readonly string[] CommitImgExtends = new String[] { "jpg", "png", "gif" };

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("上传文件")]
        public async Task<string> UploadFile(IFormFile file)
        {
            if (CommitFileExtends.Contains(Path.GetExtension(file.FileName)))
                return await ShareFile.SaveFile(file);

            throw new BussinessException("文件类型受限");
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("上传图片")]
        public Task<string> UploadImage(TInPut<string> input)
        {
            // 校验文件尾
            var ext = input.TValue.Split(',')[0].Split(';')[0].Split('/')[1];
            if (CommitImgExtends.Contains(ext))
                return ShareFile.SaveImage(input.TValue);

            throw new BussinessException("文件类型受限");
        }
        /// <summary>
        /// 生成音频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("生成音频")]
        public static Task<string> SynthesizeAudio(TInPut<string> input)
            => ShareFile.SynthesizeAudioAsync(input.TValue);
    }
}
