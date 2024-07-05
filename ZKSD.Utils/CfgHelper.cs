using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZKSD.Utils
{
    public class CfgHelper
    {
        public class LoadCfgModel
        {
            //类型
            public Type Type { get; set; }
            //对象
            public object Cfg { get; set; }
            //路径
            public string Path { get; set; }
        }

        private static readonly Dictionary<Type, LoadCfgModel> cfgs = new Dictionary<Type, LoadCfgModel>();


        public static void RegisterCfgModel(LoadCfgModel loadCfgModel)
        {
            if (cfgs.ContainsKey(loadCfgModel.Type) == false)
                cfgs.Add(loadCfgModel.Type, loadCfgModel);
        }




        static public object Get(Type type)
        {
            if (cfgs.ContainsKey(type) == false)
            {
                throw new Exception("模型路径未配置。");
            }

            LoadCfgModel cfgModel = cfgs[type];

            if (cfgModel.Cfg == null)
            {
                //加载本地配置
                object obj = LoadCfg(cfgModel.Path, type);
                if (obj == null)
                {
                    obj = Activator.CreateInstance(type);
                    Set(obj);//没有配置文件重新建立
                }
                cfgModel.Cfg = obj;
            }

            return cfgModel.Cfg;
        }
        static public void Set(object cfgObj)
        {
            Type type = cfgObj.GetType();
            if (cfgs.ContainsKey(type) == false)
            {
                throw new Exception("模型路径未配置。");
            }
            LoadCfgModel cfgModel = cfgs[type];

            //更新本地
            SaveCfg(cfgObj, cfgModel.Path);
            //更新内存
            cfgModel.Cfg = cfgObj;
        }

        static public object LoadCfg(string path, Type type)
        {
            object o = null;
            try
            {
                if (File.Exists(path))
                {
                    string s = File.ReadAllText(path);
                    o = JsonConvert.DeserializeObject(s, type);
                }
            }
            catch
            {
                o = null;
            }
            return o;
        }

        static public bool SaveCfg(object obj, string file)
        {
            try
            {
                //删除旧文件
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                //创建目录
                string dir = DirectoryHelper.GetDirectoryLastPath(file, 1);
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }
                // 写入到文件中
                string s = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(file, s);
            }
            catch (Exception e)
            {
                return false;

            }
            return true;
        }

    }
}
