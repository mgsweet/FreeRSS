using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace freeRSS.Common
{
    // 请确认电脑可以全局访问谷歌服务器，否则Feed图标将会使用默认图标

    public static class WebIconDownloadTool
    {
        /// <summary>
        /// HttpContent异步读取响应流并写入本地文件方法扩展
        /// </summary>
        public static async Task DownloadAsFileAsync(this HttpContent content, string fileName, bool overwrite)
        {
            string filePath = Path.GetFullPath(ApplicationData.Current.LocalFolder.Path + "\\" + fileName + ".png");
            Debug.WriteLine(filePath);

            if (!overwrite && File.Exists(filePath))
            {
                throw new InvalidOperationException(string.Format("文件 {0} 已经存在！", filePath));
            }

            try
            {
                await content.ReadAsByteArrayAsync().ContinueWith(
                    (readBytestTask) =>
                    {
                        byte[] data = readBytestTask.Result;
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            fs.Write(data, 0, data.Length);   
                            //防止文件被独占的。
                            fs.Dispose();
                        }
                    }
                    );
            }
            catch (Exception e)
            {
                Debug.WriteLine("发生异常： {0}", e.Message);
            }
        }

        /// <summary>
        /// 直接给出icon的URI网址进行下载
        /// 使用Feed的ID来作为Icon的名字
        /// </summary>
        public static async Task<bool> DownLoadIconFrom_IconUri(string Icon_Uri, string IconName)
        {
            const string GOOGLE_ICON_SEARCH_PRE = @"http://www.google.com/s2/favicons?domain_url=";

            string reqUrl = GOOGLE_ICON_SEARCH_PRE + Icon_Uri;
            HttpClient httpClient = new HttpClient();
            var cancellationTokenSource = new CancellationTokenSource(2000);
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(reqUrl, cancellationTokenSource.Token);
                response.EnsureSuccessStatusCode();
                await response.Content.DownloadAsFileAsync(IconName, true).ContinueWith(
                            (readTask) =>
                            {
                                Debug.WriteLine("文件下载完成！");
                            });
                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine("Try to DownLoad Favicon Failed! Check Your Internet Or Google May be Blocked!");
                return false;
            }
        }
    }
}
