using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NetCoreObject.Common
{
    /// <summary>
    /// 数据库备份
    /// </summary>
    public class DataHelper
    {
        public static IConfiguration _config;
        public static void GetIntance(IConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// 数据库备份操作
        /// </summary>
        /// <param name="SqlNumber"></param>
        public static void BakBackUpFun(string SqlNumber)
        {
            var ConnStr = _config.GetConnectionString("DefaultConnection");
            string[] result = ConnStr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split('=')[1]).ToArray();

            var filePath = Utils.GetMapPath("/wwwroot/upload/backdb/");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            StringBuilder sbcommand = new StringBuilder();
            string fileName = Utils.GetMapPath("/wwwroot/upload/backdb/" + result[3] + "_" + SqlNumber + ".sql");
            //sbcommand.AppendFormat("mysqldump --quick --host=localhost --default-character-set=utf8 --lock-tables --verbose  --force --port=3306 --user=root --password=123456 fytsoadb -r \"{0}\"", fileName);
            //sbcommand.AppendFormat("mysqldump.exe --quick --host=\"{0}\" --default-character-set=\"{1}\" --lock-tables --verbose --force --port=3306 --user=\"{2}\" --password=\"{3}\" \"{4}\" -r \"{5}\"", result[0], result[4].ToLower(), result[1], result[2], result[3], fileName);

            sbcommand.AppendFormat("mysqldump  -h {0} -u{1} -p{2} --default-character-set={3} --opt --disable-keys --lock-all-tables -R --hex-blob  {4} >{5}", result[0], result[1], result[2], result[4].ToLower(), result[3], fileName);

            String command = sbcommand.ToString();

            //String appDirecroty = Utils.GetMapPath("/XmlConfig/");

            var endStr = FytRequest.RunCmd(command);
        }
    }
}
