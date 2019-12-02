using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace thirdCamera
{
    public partial class Form1 : Form
    {
        private bool loadCompleted = false;
        /// <summary>  
        /// 修改注册表信息来修改程序默认集成的浏览器内核版本
        /// </summary>  
        static void SetWebBrowserFeatures(int ieVersion)
        {
            // don't change the registry if running in-proc inside Visual Studio  
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;
            //获取程序及名称  
            var appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            //得到浏览器的模式的值  
            UInt32 ieMode = GeoEmulationModee(ieVersion);
            var featureControlRegKey = @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\";
            //设置浏览器对应用程序（appName）以什么模式（ieMode）运行  
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION",
                appName, ieMode, RegistryValueKind.DWord);
            // enable the features which are "On" for the full Internet Explorer browser  
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION",
                appName, 1, RegistryValueKind.DWord);
        }

        /// <summary>  
        /// 通过版本得到浏览器模式的值  
        /// </summary>  
        /// <param name="browserVersion"></param>  
        /// <returns></returns>  
        static UInt32 GeoEmulationModee(int browserVersion)
        {
            UInt32 mode = 11000; // Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 Standards mode.   
            switch (browserVersion)
            {
                case 7:
                    mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode.   
                    break;
                case 8:
                    mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode.   
                    break;
                case 9:
                    mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode.                      
                    break;
                case 10:
                    mode = 10000; // Internet Explorer 10.  
                    break;
                case 11:
                    mode = 11000; // Internet Explorer 11  
                    break;
            }
            return mode;
        }
        public Form1()
        {
            SetWebBrowserFeatures(11);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = @"D:\高拍仪测试\liangtian\liangtian.html";
            this.webBrowser1.Navigate(url);
            this.webBrowser1.ScriptErrorsSuppressed = false;
            this.webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);//加载完成后的事件
        }

        private void wb_DocumentCompleted(object sender, EventArgs e)
        {
            loadCompleted = true;
            var index = 0;
            string resolution = "320x240";
            webBrowser1.Document.InvokeScript("DeviceLoad", new Object[] { index, resolution });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.loadCompleted)//在文档加载完毕后调用
            {
                string imgPath = @"D:\高拍仪测试\liangtian\picture\";
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                string saveType = "jpg";
                if (!Directory.Exists(imgPath))
                {
                    Directory.CreateDirectory(imgPath);
                }
                webBrowser1.Document.InvokeScript("takePhoto", new Object[] { imgPath, fileName, saveType });
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string url = "https://ie.icoa.cn/";
            this.webBrowser1.Navigate(url);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPropertiesDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string url = @"D:\高拍仪测试\liangtian\liangtian.html";
            this.axWebBrowser1.Navigate(url);
            //this.webBrowser1.ScriptErrorsSuppressed = false;
            //this.webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted1);
        }
    }
}
