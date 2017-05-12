﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace freeRSS.Common
{
    // 大多数网站直接不明确写明icon,但可通过在url上加上"/favicon.ico"访问获取
    // 格式1：<link rel="shortcut icon" href="https://static.zhihu.com/static/favicon.ico" type="image/x-icon">
    // 格式2：<link rel="shortcut icon" href="/favicon.ico" type="image/x-icon">
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
        /// 判断是不是图片文件
        /// </summary>
        private static bool IsPicture(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryReader reader = new BinaryReader(fs);
                    string fileClass;
                    byte buffer;
                    buffer = reader.ReadByte();
                    fileClass = buffer.ToString();
                    buffer = reader.ReadByte();
                    fileClass += buffer.ToString();
                    reader.Dispose();
                    fs.Dispose();
                    if (fileClass == "255216" || fileClass == "7173" || fileClass == "13780" || fileClass == "6677")

                    //255216是jpg;7173是gif;6677是BMP,13780是PNG;7790是exe,8297是rar 
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 下载给定URI网址对应的icon, 例入传入"http://www.126.com/"
        /// </summary>
        public static async Task DownLoadIconFrom_WebUri(string Uri, string IconName)
        {
            // 大多数网站直接不明确写明icon,但可通过在url上加上"/favicon.ico"访问获取
            string IconUri = Uri + "favicon.ico";
            try
            {
                // 格式1
                await DownLoadIconFrom_IconUri(IconUri, IconName);
                // 格式2
                if(!IsPicture(ApplicationData.Current.LocalFolder.Path + "\\" + IconName + ".png")) {
                    Debug.WriteLine("格式一获取失败");
                    await getIconByFurtherSearch(Uri, IconName);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 直接给出icon的URI网址进行下载
        /// </summary>
        public static async Task DownLoadIconFrom_IconUri(string Icon_Uri, string IconName)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(Icon_Uri);

            response.EnsureSuccessStatusCode();
            await response.Content.DownloadAsFileAsync(IconName, true).ContinueWith(
                        (readTask) =>
                        {
                            Debug.WriteLine("文件下载完成！");
                        });
        }

        /// <summary>
        /// 解决少部分网站没有"/favicon.ico"的问题，通过html内容获取
        /// </summary>
        /// <param name="Uri"></param>
        /// <returns></returns>
        private static async Task getIconByFurtherSearch(string Uri, string IconName)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                // 创建一个异步GET请求，当请求返回时继续处理（Continue-With模式）

                HttpResponseMessage response = await httpClient.GetAsync(Uri);
                string resultStr = await response.Content.ReadAsStringAsync();
                string patternOfShortCut = "<link +rel=\"shortcut icon\" +href=\"([^\"]*)\"";

                // 正则匹配
                var regex = new Regex(patternOfShortCut, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                var matchStrings = regex.Matches(resultStr);

                // 取子表达式
                string IconUri;
                if (matchStrings.Count > 0)
                {
                    IconUri = matchStrings[0].Groups[1].Value;
                    Debug.WriteLine("Get Icon Uri: " + IconUri);
                    await DownLoadIconFrom_IconUri(IconUri, IconName);
                }
                else
                {
                    Debug.WriteLine("No icon uri!");
                    IconUri = "";
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
