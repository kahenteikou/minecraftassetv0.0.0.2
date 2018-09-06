using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace MCVERJSONLib
{
    public class MCJSON1
    {
        public string assets { get; set; }


    }
    public class Core
    {
        public void TEST1()
        {
            string JSONpath = @"C:\Users\kokki\AppData\Roaming\.minecraft\versions\1.12.2\1.12.2.json";
                 StreamReader sr = new StreamReader(JSONpath);
            string text = sr.ReadToEnd(); //読み込み!
            //デシリアライズ
            var deserialized = JsonConvert.DeserializeObject<MCJSON1>(text);
            MCJSON1 JSON1 = deserialized;
            MessageBox.Show(JSON1.assets);
        }
        public string JSONAssetVer(string Verpath)
        {
            string patha = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // Appdataを取得
            string patha2 = patha + "\\.minecraft\\versions\\" + Verpath + "\\" + Verpath + ".json"; //JSONファイルを取得
            StreamReader sr = new StreamReader(patha2); //Stream作成
            string text = sr.ReadToEnd(); //読み込み!
            //デシリアライズ
            var deserialized = JsonConvert.DeserializeObject<MCJSON1>(text);
            MCJSON1 JSON1 = deserialized;
            string retA = JSON1.assets;
            return retA;



        }
    }
}
