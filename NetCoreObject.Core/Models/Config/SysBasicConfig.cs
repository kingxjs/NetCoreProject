using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreObject.Core
{
    /// <summary>
    /// 站点配置实体类
    /// </summary>
    [Serializable]
    public class SysBasicConfig
    {
        public SysBasicConfig()
        { }

        private string _webcopyright = "";

        private string _webpath = "";
        private string _webmanagepath = "";
        private int _staticstatus = 0;
        private int _memberstatus = 1;
        private int _commentstatus = 0;
        private int _logstatus = 0;
        private int _webstatus = 1;
        private string _webclosereason = "";
        private string _webcountcode = "";

        private string _smsapiurl = "";
        private string _smsusername = "";
        private string _smspassword = "";

        private string _emailsmtp = "";
        private int _emailport = 25;
        private string _emailfrom = "";
        private string _emailusername = "";
        private string _emailpassword = "";
        private string _emailnickname = "";
        private bool _emailssl = false;

        private string _filepath = "";
        private int _filesave = 1;
        private string _fileextension = "";
        private int _attachsize = 0;
        private int _imgsize = 0;
        private int _imgmaxheight = 0;
        private int _imgmaxwidth = 0;
        private int _thumbnailheight = 0;
        private int _thumbnailwidth = 0;
        private int _watermarktype = 0;
        private int _watermarkposition = 9;
        private int _watermarkimgquality = 80;
        private string _watermarkpic = "";
        private int _watermarktransparency = 10;
        private string _watermarktext = "";
        private string _watermarkfont = "";
        private int _watermarkfontsize = 12;

        private string _sysdatabaseprefix = "dt_";
        private string _sysencryptstring = "Fytcms";

        private string _wxewcode = "";
        public int _vipmoney = 0;
        public string _vipSummary = "";

        #region 网站基本信息==================================

        /// <summary>
        /// 网站版权信息
        /// </summary>
        public string webcopyright
        {
            get { return _webcopyright; }
            set { _webcopyright = value; }
        }
        #endregion

        #region 功能权限设置==================================
        /// <summary>
        /// 是否开启统计功能
        /// </summary>
        public int staticstatus
        {
            get { return _staticstatus; }
            set { _staticstatus = value; }
        }
        /// <summary>
        /// 开启会员功能
        /// </summary>
        public int memberstatus
        {
            get { return _memberstatus; }
            set { _memberstatus = value; }
        }
        /// <summary>
        /// 开启评论审核
        /// </summary>
        public int commentstatus
        {
            get { return _commentstatus; }
            set { _commentstatus = value; }
        }
        /// <summary>
        /// 后台管理日志
        /// </summary>
        public int logstatus
        {
            get { return _logstatus; }
            set { _logstatus = value; }
        }
        /// <summary>
        /// 是否关闭网站
        /// </summary>
        public int webstatus
        {
            get { return _webstatus; }
            set { _webstatus = value; }
        }
        /// <summary>
        /// 关闭原因描述
        /// </summary>
        public string webclosereason
        {
            get { return _webclosereason; }
            set { _webclosereason = value; }
        }
        /// <summary>
        /// 网站统计代码
        /// </summary>
        public string webcountcode
        {
            get { return _webcountcode; }
            set { _webcountcode = value; }
        }
        #endregion

        #region 短信平台设置==================================
        /// <summary>
        /// 短信API地址
        /// </summary>
        public string smsapiurl
        {
            get { return _smsapiurl; }
            set { _smsapiurl = value; }
        }
        /// <summary>
        /// 短信平台登录账户名
        /// </summary>
        public string smsusername
        {
            get { return _smsusername; }
            set { _smsusername = value; }
        }
        /// <summary>
        /// 短信平台登录密码
        /// </summary>
        public string smspassword
        {
            get { return _smspassword; }
            set { _smspassword = value; }
        }
        #endregion

        #region 邮件发送设置==================================
        /// <summary>
        /// STMP服务器
        /// </summary>
        public string emailsmtp
        {
            get { return _emailsmtp; }
            set { _emailsmtp = value; }
        }
        /// <summary>
        /// SMTP端口
        /// </summary>
        public int emailport
        {
            get { return _emailport; }
            set { _emailport = value; }
        }
        /// <summary>
        /// 发件人地址
        /// </summary>
        public string emailfrom
        {
            get { return _emailfrom; }
            set { _emailfrom = value; }
        }
        /// <summary>
        /// 邮箱账号
        /// </summary>
        public string emailusername
        {
            get { return _emailusername; }
            set { _emailusername = value; }
        }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string emailpassword
        {
            get { return _emailpassword; }
            set { _emailpassword = value; }
        }
        /// <summary>
        /// 发件人昵称
        /// </summary>
        public string emailnickname
        {
            get { return _emailnickname; }
            set { _emailnickname = value; }
        }
        /// <summary>
        /// 邮件SSL
        /// </summary>
        public bool emailssl
        {
            get { return _emailssl; }
            set { _emailssl = value; }
        }
        #endregion

        #region 文件上传设置==================================
        /// <summary>
        /// 上传目录根目录
        /// </summary>
        public string filerootpath { get; set; } = "wwwroot";
        /// <summary>
        /// 附件上传目录
        /// </summary>
        public string filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }
        /// <summary>
        /// 附件保存方式
        /// </summary>
        public int filesave
        {
            get { return _filesave; }
            set { _filesave = value; }
        }
        /// <summary>
        /// 附件上传类型
        /// </summary>
        public string fileextension
        {
            get { return _fileextension; }
            set { _fileextension = value; }
        }
        /// <summary>
        /// 文件上传大小
        /// </summary>
        public int attachsize
        {
            get { return _attachsize; }
            set { _attachsize = value; }
        }
        /// <summary>
        /// 图片上传大小
        /// </summary>
        public int imgsize
        {
            get { return _imgsize; }
            set { _imgsize = value; }
        }
        /// <summary>
        /// 图片最大高度(像素)
        /// </summary>
        public int imgmaxheight
        {
            get { return _imgmaxheight; }
            set { _imgmaxheight = value; }
        }
        /// <summary>
        /// 图片最大宽度(像素)
        /// </summary>
        public int imgmaxwidth
        {
            get { return _imgmaxwidth; }
            set { _imgmaxwidth = value; }
        }
        /// <summary>
        /// 生成缩略图高度(像素)
        /// </summary>
        public int thumbnailheight
        {
            get { return _thumbnailheight; }
            set { _thumbnailheight = value; }
        }
        /// <summary>
        /// 生成缩略图宽度(像素)
        /// </summary>
        public int thumbnailwidth
        {
            get { return _thumbnailwidth; }
            set { _thumbnailwidth = value; }
        }
        /// <summary>
        /// 图片水印类型
        /// </summary>
        public int watermarktype
        {
            get { return _watermarktype; }
            set { _watermarktype = value; }
        }
        /// <summary>
        /// 图片水印位置
        /// </summary>
        public int watermarkposition
        {
            get { return _watermarkposition; }
            set { _watermarkposition = value; }
        }
        /// <summary>
        /// 图片生成质量
        /// </summary>
        public int watermarkimgquality
        {
            get { return _watermarkimgquality; }
            set { _watermarkimgquality = value; }
        }
        /// <summary>
        /// 图片水印文件
        /// </summary>
        public string watermarkpic
        {
            get { return _watermarkpic; }
            set { _watermarkpic = value; }
        }
        /// <summary>
        /// 水印透明度
        /// </summary>
        public int watermarktransparency
        {
            get { return _watermarktransparency; }
            set { _watermarktransparency = value; }
        }
        /// <summary>
        /// 水印文字
        /// </summary>
        public string watermarktext
        {
            get { return _watermarktext; }
            set { _watermarktext = value; }
        }
        /// <summary>
        /// 文字字体
        /// </summary>
        public string watermarkfont
        {
            get { return _watermarkfont; }
            set { _watermarkfont = value; }
        }
        /// <summary>
        /// 文字大小(像素)
        /// </summary>
        public int watermarkfontsize
        {
            get { return _watermarkfontsize; }
            set { _watermarkfontsize = value; }
        }
        #endregion

        #region 版本设置================================
        /// <summary>
        /// ios版本号
        /// </summary>
        public string iosVersionCode { get; set; }

        /// <summary>
        /// ios是否升级
        /// </summary>
        public string iosisUpgrade { get; set; }

        /// <summary>
        /// ios审核开关
        /// </summary>
        public string iosAudit { get; set; }

        /// <summary>
        /// ios升级文件地址
        /// </summary>
        public string iosUgradeFile { get; set; }

        /// <summary>
        /// ios升级当前内容介绍
        /// </summary>
        public string iosUgradeSummary { get; set; }

        /// <summary>
        /// 安卓版本号
        /// </summary>
        public string androidVersionCode { get; set; }

        /// <summary>
        /// 安卓升级文件
        /// </summary>
        public string androidUgradeFile { get; set; }

        /// <summary>
        /// 安卓升级当前内容介绍
        /// </summary>
        public string androidUgradeSummary { get; set; }
        #endregion
    }
}
