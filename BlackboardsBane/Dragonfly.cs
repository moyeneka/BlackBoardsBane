using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackboardsBane
{
    //helper for cefsharp with random snipets
    //I have no idea why I called it dragonfly
    public class DragonFlyCEF
    {
        public ChromiumWebBrowser browser;

        public delegate void EmptyEventHandler(object sender);
        public event EmptyEventHandler BrowserLoaded = delegate { };
        public event EmptyEventHandler DOMLoaded = delegate { };

        public DragonFlyCEF()
        {
            var settings = new CefSettings()
            {
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DragonFly\\Cache")
            };
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            browser = new ChromiumWebBrowser();
            browser.BrowserInitialized += BrowserInitialized;
            browser.LoadingStateChanged += LoadingStateChanged;
        }

        public void Connect(string url)
        {
            browser.Load(url);
        }

        public async Task<string> GetTitle(bool current = true)
        {
            var frame = browser.GetMainFrame();
            if (current)
            {
                return (string)await ExecuteJs("return document.title;");
            }
            else
            {
                var hasTitleTag = (bool)await ExecuteJs("return document.getElementsByTagName(\"title\").length > 0;");
                if (hasTitleTag)
                {
                    return (string)await ExecuteJs("return document.getElementsByTagName(\"title\")[0].innerHTML;");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public async Task<bool> ElementExists(string id)
        {
            return (bool)await ExecuteJs($"return document.getElementById(\"{id}\") != null");
        }

        public async Task<object> ExecuteJs(string action)
        {
            var frame = browser.GetMainFrame();
            var task = await frame.EvaluateScriptAsync($"(function() {{ {action} }})();", null);

            var response = task.Result;
            if (task.Success)
            {
                return response ?? null;
            }
            else
            {
                throw new Exception(task.Message);
            }
        }

        private void BrowserInitialized(object sender, EventArgs e)
        {
            BrowserLoaded(this);
        }

        private void LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                DOMLoaded(this);
            }
        }
    }
}
