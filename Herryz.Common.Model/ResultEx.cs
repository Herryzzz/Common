using System;

namespace Herryz.Common.Model
{
    public class ResultEx
    {
        /// <summary>
        /// 是否操作成功
        /// </summary>
        public bool flag { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public object msg { get; set; }
        /// <summary>
        /// 其它信息
        /// </summary>
        public object data { get; set; }

        public ResultEx()
        {
            this.flag = true;
            this.msg = string.Empty;
            this.data = string.Empty;
        }
        /// <summary>
        /// 创建一个ResultEx 返回正确
        /// </summary>
        /// <returns></returns>
        public static ResultEx Init()
        {
            return Init(true, string.Empty);
        }
        /// <summary>
        /// 创建一个ResultEx 返回false
        /// </summary>
        /// <param name="msg">提示的消息</param>
        /// <returns></returns>
        public static ResultEx Init(string msg)
        {
            return Init(false, msg);
        }
        /// <summary>
        /// 创建一个ResultEx 返回false
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ResultEx Init(Exception ex)
        {
            return Init(false, ex.Message);
        }
        /// <summary>
        /// 创建一个ResultEx
        /// </summary>
        /// <param name="flag">是否正确</param>
        /// <returns></returns>
        public static ResultEx Init(bool flag)
        {
            return new ResultEx() { flag = flag};
        }
        /// <summary>
        /// 创建一个ResultEx
        /// </summary>
        /// <param name="flag">是否正确</param>
        /// <param name="msg">提示的消息</param>
        /// <returns></returns>
        public static ResultEx Init(bool flag, string msg)
        {
            return new ResultEx() { flag = flag, msg = msg };
        }
        /// <summary>
        /// 创建一个ResultEx
        /// </summary>
        /// <param name="flag">是否正确</param>
        /// <param name="msg">提示的消息</param>
        /// <param name="data">其它信息</param>
        /// <returns></returns>
        public static ResultEx Init(bool flag, string msg, object data)
        {
            return new ResultEx() { flag = flag, msg = msg, data = data };
        }
    }
}
