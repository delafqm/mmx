using mmx.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace mmx
{
    public class Speech
    {
        static string APP_ID = "14965195";
        static string API_KEY = "R2qXXgwr9xKtge3kxU5U7up2";
        static string SECRET_KEY = "Gnm2KhHcgZEDDLwy0Qtl66y4fFc8FmTj";

        public static async Task<string> GetToken()
        {
            string UrlBase = "http://openapi.baidu.com/oauth/2.0/token";
            //var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            HttpClient http = new HttpClient();

            string UrlParams = "grant_type=client_credentials" + "&";
            UrlParams += "client_id=R2qXXgwr9xKtge3kxU5U7up2" + "&";
            UrlParams += "client_secret=Gnm2KhHcgZEDDLwy0Qtl66y4fFc8FmTj";
            string url = UrlBase + "?" + UrlParams;
            //result = UrlBase + "?" + UrlParams;
            try
            {
                HttpResponseMessage response = await http.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    string Result = await response.Content.ReadAsStringAsync();

                    var Item = JsonConvert.DeserializeObject<BaiduToken>(Result);

                    return Item.access_token;
                }

                return "error";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 语音识别，语音转文本
        /// </summary>
        public static async Task<SpeechResult> Asr(string filepath)
        {
            SpeechResult result = new SpeechResult();
            if (File.Exists(filepath))
            {
                //var APP_ID = "14965195";
                //var API_KEY = "R2qXXgwr9xKtge3kxU5U7up2";
                //var SECRET_KEY = "Gnm2KhHcgZEDDLwy0Qtl66y4fFc8FmTj";
                var client = new Baidu.Aip.Speech.Asr(APP_ID, API_KEY, SECRET_KEY);
                client.Timeout = 60000;  // 修改超时时间

                //读取文件
                //string rootPath = Directory.GetCurrentDirectory();
                var data = File.ReadAllBytes(filepath);

                //识别语种，英文1737;
                Dictionary<string, object> op = new Dictionary<string, object>();
                op["dev_pid"] = 1737;

                //client.Timeout = 120000; // 若语音较长，建议设置更大的超时时间. ms

                var res = Task.Run(() =>
                {
                    var result1 = client.Recognize(data, "amr", 16000, op);

                    MResult mResult = JsonConvert.DeserializeObject<MResult>(result1.ToString());
                    if (mResult.err_no == 0)
                    {
                        //设置成功返回数据
                        result.status = 0;
                        result.text = mResult.result[0].ToString();
                    }
                    else
                    {
                        //设置失败返回数据
                        result.status = 1;
                        result.error = "语音错误：" + mResult.err_no.ToString();
                    }
                });
                await res;
            }
            else
            {
                //失败数据
                result.status = 1;
                result.error = "语音错误：无语音";
            }

            return result;
        }

        /// <summary>
        /// 语音合成，文本转语音
        /// </summary>
        /// <param name="text"></param>
        /// <param name="spd"></param>
        /// <param name="pit"></param>
        /// <returns></returns>
        public static async Task<SpeechResult> Tts(string text,int spd,int pit)
        {
            SpeechResult result = new SpeechResult();

            //var APP_ID = "14965195";
            //var API_KEY = "R2qXXgwr9xKtge3kxU5U7up2";
            //var SECRET_KEY = "Gnm2KhHcgZEDDLwy0Qtl66y4fFc8FmTj";
            var client = new Baidu.Aip.Speech.Tts(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间

            // 可选参数
            var option = new Dictionary<string, object>()
            {
                {"spd", spd}, // 语速
                {"vol", 7}, // 音量
                {"pit", pit}, // 语调
                {"per", 0},  // 发音人，0为普通女声，1为普通男生，3为情感合成-度逍遥，4为情感合成-度丫丫，默认为普通女声
                {"aue", 3}
            };

            var res = Task.Run(() =>
            {
                var res1 = client.Synthesis(text, option);


                if (res1.ErrorCode == 0)  // 或 result.Success
                {
                    //成功数据
                    result.status = 0;
                    result.speech = res1.Data;
                    //File.WriteAllBytes("合成的语音文件本地存储地址.mp3", result.Data);
                }
                else
                {
                    //失败数据
                    result.status = 1;
                    result.error = "错误:" + res1.ErrorCode.ToString();
                }
            });
            await res;

            return result;
        }


        /// <summary>
        /// API获取语音，文本转语音
        /// </summary>
        /// <param name="text">需要转换语音的文本</param>
        /// <param name="vol">音量</param>
        /// <param name="per">发间人</param>
        /// <param name="spd">语速</param>
        /// <param name="pit">语调</param>
        /// <returns></returns>
        public static async Task<SpeechResult> TextToSpeech1(string text, string vol, string per, string spd, string pit, string token)
        {

            SpeechResult result = new SpeechResult();
            string url = "http://tsn.baidu.com/text2audio"; //百度tts请求地址
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 1, 0); //超时时间设置aue


            Dictionary<string, string> param = new Dictionary<string, string>() {
                { "lan", "zh" },
                { "ctp", "1" },
                { "vol", vol },  //音量：0-15，默认5中音量
                { "per", per },   //发音人：0为普通女声，1为普通男声，3为情感合成-度逍遥，4为情感合成-度丫丫
                { "spd", spd },   //语速：0-15，5为中语速
                { "pit", pit },   //音调：0-15，5为中语调
                { "aue","3"}    //不需要修改的参数
            };
            param.Add("tex", text); //需要转换的文本内容
            param.Add("cuid", "delafqmisspeed"); //用户唯一标识"24.021dc614e13e2f98f43b661fb40495d5.2592000.1557308095.282335-14965195"
            param.Add("tok", token);//access_token，在发送之前先调用一次获取到这个值
            FormUrlEncodedContent content = new FormUrlEncodedContent(param); //post请求参数设置对象

            try
            {
                HttpResponseMessage x = await client.PostAsync(url, content);
                if (x.StatusCode == HttpStatusCode.OK)
                {
                    #region 错误处理
                    HttpContentHeaders header = x.Content.Headers;
                    //如果返回错误信息
                    if (header.ContentType.ToString() == "application/json")
                    {
                        string res = await x.Content.ReadAsStringAsync();

                        var Item = JsonConvert.DeserializeObject<TResult>(res);

                        //设置失败返回数据
                        result.status = 1;
                        result.error = Item.err_no.ToString();
                    }
                    #endregion

                    var resbyte = await x.Content.ReadAsByteArrayAsync();
                    //设置成功返回数据
                    result.status = 0;
                    result.speech = resbyte;
                }
                else
                {
                    //失败
                    result.status = 1;
                    result.error = "请求失败";
                }
            }
            catch (Exception ex)
            {
                //失败
                result.status = 1;
                result.error = ex.Message;
            }

            return result;
        }


        #region 迅飞语音
        public static String Md5(string s)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();
            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }
            return ret.PadLeft(32, '0');
        }

        public static string Headers(string input,string filepath,string spd)
        {

            string appID = "5caaefc3";
            string APIKey = "4fb15435a527621199d0287ade092ca5";
            String url = "http://api.xfyun.cn/v1/service/v1/tts";
            string result = "";

            String bodys;
            string text = input;
            //对要合成语音的文字先用utf-8然后进行URL加密
            byte[] textData = Encoding.UTF8.GetBytes(text);
            text = HttpUtility.UrlEncode(textData);
            bodys = string.Format("text={0}", text);


            //aue = raw, 音频文件保存类型为 wav
            //aue = lame, 音频文件保存类型为 mp3
            string AUE = "lame";
            string param = "{\"aue\":\"" + AUE + "\",\"auf\":\"audio/L16;rate=16000\",\"voice_name\":\"x_catherine\",\"speed\":\"" + spd + "\",\"engine_type\":\"intp65_en\"}";
            //获取十位的时间戳
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string curTime = Convert.ToInt64(ts.TotalSeconds).ToString();
            //对参数先utf-8然后用base64编码
            byte[] paramData = Encoding.UTF8.GetBytes(param);
            string paraBase64 = Convert.ToBase64String(paramData);
            //形成签名
            string checkSum = Md5(APIKey + curTime + paraBase64);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("X-Param", paraBase64);
            request.Headers.Add("X-CurTime", curTime);
            request.Headers.Add("X-Appid", appID);
            request.Headers.Add("X-CheckSum", checkSum);

            Stream requestStream = request.GetRequestStream();
            StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.GetEncoding("gb2312"));
            streamWriter.Write(bodys);
            streamWriter.Close();

            String htmlStr = string.Empty;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
            {
                string header_type = response.Headers["Content-Type"];
                if (header_type == "audio/mpeg")
                {
                    Stream st = response.GetResponseStream();
                    MemoryStream memoryStream = StreamToMemoryStream(st);
                    File.WriteAllBytes(filepath, streamTobyte(memoryStream));
                    //Console.WriteLine(response.Headers);
                    //Console.ReadLine();
                    result = "success";
                }
                else
                {
                    result = reader.ReadToEnd();
                }
            }
            responseStream.Close();
            return result;
        }


        #region 把流转换成缓存流
        static MemoryStream StreamToMemoryStream(Stream instream)
        {
            MemoryStream outstream = new MemoryStream();
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = instream.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
            }
            return outstream;
        }
        #endregion

        #region 把缓存流转换成字节组
        public static byte[] streamTobyte(MemoryStream memoryStream)
        {
            byte[] buffer = new byte[memoryStream.Length];
            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        #endregion
        #endregion
    }
}
