using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreObject.Common
{
    public class BasicConfigHelper
    {
        public static SysBasicConfig LoadConfig(string configFilePath)
        {
            return (SysBasicConfig)SerializationHelper.Load(typeof(SysBasicConfig), configFilePath);
        }
        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <returns></returns>
        public async static Task<SysBasicConfig> getBasicConfig()
        {
            var model = RedisHelper.StringGetAsync<SysBasicConfig>("system:" + KeyHelper.CACHE_SITE_CONFIG);

            if (model != null) return model.Result;
            await RedisHelper.StringSetAsync("system:" + KeyHelper.CACHE_SITE_CONFIG, LoadConfig(Utils.GetXmlMapPath(KeyHelper.FILE_SITE_XML_CONFING)));
            model = RedisHelper.StringGetAsync<SysBasicConfig>("system:" + KeyHelper.CACHE_SITE_CONFIG);

            return model.Result;
        }
        /// <summary>
        /// 刷新配置项
        /// </summary>
        public static Task<bool> setBasicConfig()
        {
            return RedisHelper.StringSetAsync("system:" + KeyHelper.CACHE_SITE_CONFIG, LoadConfig(Utils.GetXmlMapPath(KeyHelper.FILE_SITE_XML_CONFING)));
        }
    }
}
