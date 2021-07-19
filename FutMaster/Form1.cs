using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Threading;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Globalization;

namespace FutMaster
{
    public partial class Form1 : Form
    {

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);

        public ChromiumWebBrowser browser;

        public enum Page { Home, Squads, SBC, Transfers, Store, Club, Leaderboards, Settings }

        Page currentPage = new Page();
        bool isLoaded = false;
        bool botEnabled = false;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        public bool wait = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
		    Cef.Initialize(new CefSettings());
			browser = new ChromiumWebBrowser("https://signin.ea.com/p/web2/login?execution=e833872840s1&initref=https%3A%2F%2Faccounts.ea.com%3A443%2Fconnect%2Fauth%3Fprompt%3Dlogin%26accessToken%3D%26client_id%3DFIFA21_JS_WEB_APP%26response_type%3Dtoken%26display%3Dweb2%252Flogin%26locale%3Den_US%26redirect_uri%3Dhttps%253A%252F%252Fwww.ea.com%252Ffifa%252Fultimate-team%252Fweb-app%252Fauth.html%26release_type%3Dprod%26scope%3Dbasic.identity%2Boffline%2Bsignin%2Bbasic.entitlement%2Bbasic.persona");
            browser.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(browser);
            currentPage = Page.Home;
            /*shortcutsWorker.Dispose();
            shortcutsWorker.DoWork += ShortcutsWork;
            shortcutsWorker.RunWorkerAsync();*/
            //loadingWorker.Dispose();
            //loadingWorker.DoWork += WaitForLoaded;
            //loadingWorker.RunWorkerAsync();
            statusLabel.Text = "Waiting for login";
        }

        private int randomDelay(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        private void AutoBuy(object sender, EventArgs e)
        {
            MessageBox.Show("STARTED");
            while (wait == false && botEnabled == true && mainWorker.CancellationPending == false)
            {
                wait = true;
                while (GetTextByClass(browser, "title", 0) != "SEARCH THE TRANSFER MARKET")
                {
                    if (mainWorker.CancellationPending == false)
                    {
                        Thread.Sleep(100);
                    }
                    else
                    {
                        break;
                    }
                }               
                
                Thread.Sleep(randomDelay(Properties.Settings.Default.minDelay, Properties.Settings.Default.maxDelay));

                int currentValue;
                string value;

                while(GetValueByClass(browser, "numericInput", 0) == null)
                {
                    if (mainWorker.CancellationPending == false)
                    {
                        Thread.Sleep(10);
                        
                    }
                    else
                    {
                        break;
                    }

                }

                while (GetValueByClass(browser, "numericInput filled", 0) == null)
                {
                    if (mainWorker.CancellationPending == false)
                    {
                        Thread.Sleep(10);

                    }
                    else
                    {
                        break;
                    }
                }

                value = GetValueByClass(browser, "numericInput", 0);

                Random rnd = new Random();
                int max = int.Parse(GetValueByClass(browser, "numericInput filled", GetElementCountByClass(browser, "numericInput filled") - 1).Replace(",",""));
                int number = rnd.Next(150 / 50, 2000 / 50) * 50;

                browser.EvaluateScriptAsync("document.getElementsByClassName('numericInput')[0].value = " + number);
                Thread.Sleep(100);
                browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-standard increment-value')[0]);");
                Thread.Sleep(100);
                browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-standard decrement-value')[0]);");

                Thread.Sleep(200);
                browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-standard call-to-action')[0]);");
                label4.Text = "Recherche";
                while(GetTextByClass(browser, "title", 0) != "SEARCH RESULTS")
                {
                    if (mainWorker.CancellationPending == false)
                    {
                        Thread.Sleep(100);

                    }
                    else
                    {
                        break;
                    }
                }
                Thread.Sleep(250);
                int count = GetElementCountByClass(browser, "rowContent has-tap-callback");

                if(count > 0)
                {
                    for(int i = 0; i < count; i++)
                    {
                        browser.EvaluateScriptAsync(string.Format("tapElement(document.getElementsByClassName('rowContent has-tap-callback')[{0}]);", (i - 1)));
                        label4.Text = "Buy";
                        while(GetElementCountByClass(browser,"btn-standard buyButton currency-coins") < 1)
                        {
                            if (mainWorker.CancellationPending == false)
                            {
                                Thread.Sleep(10);

                            }
                            else
                            {
                                break;
                            }
                        }
                        Thread.Sleep(50);
                        browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-standard buyButton currency-coins')[0]);");
                        Thread.Sleep(50);
                        label4.Text = "Press OK";
                        for (int z = 0; z < GetElementCountByClass(browser, "btn-text"); z++)
                        {
                            if(GetTextByClass(browser,"btn-text",z).ToUpper() == "OK")
                            {
                                browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-text')[" + z.ToString() +"]);");
                            }
                        }
                        label4.Text = "acheté";
                        Thread.Sleep(50);
                        browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-text')[0]);");
                        Thread.Sleep(500);
                        browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-text')[7]);");
                        Thread.Sleep(200);
                    }
                }

                Thread.Sleep(500);
                while (GetTextByClass(browser, "title", 0) != "SEARCH RESULTS")
                {
                    if (mainWorker.CancellationPending == false)
                    {
                        Thread.Sleep(10);

                    }
                    else
                    {
                        break;
                    }
                }
                Thread.Sleep(200);
                browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('ut-navigation-button-control')[0]);");

                wait = false;
            }
        }

        private void WaitForLoaded(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.autofill == true)
            {
                bool isFilled = false;
                while (isFilled == false)
                {

                    

                    try
                    {
                        if (GetElementValueById(browser, "email") == "")
                        {
                            browser.EvaluateScriptAsync("document.getElementById('email').value = '" + Properties.Settings.Default.email + "';");
                            browser.EvaluateScriptAsync("document.getElementById('password').value = '" + Properties.Settings.Default.password + "';");
                            isFilled = true;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            while (isLoaded == false)
            {
                try
                {
                    if (GetTextByClass(browser, "title", 0) == "HOME")
                    {
                        isLoaded = true;
                        statusLabel.Text = "Idle";
                    }
                }
                catch (Exception)
                {

                }
            }

            string script = @"function sendTouchEvent(element, eventType) {
            const touchObj = new Touch({
              identifier: 'Keyboard shortcuts should be supported natively without an extension!',
              target: element,
              clientX: 0,
              clientY: 0,
              radiusX: 2.5,
              radiusY: 2.5,
              rotationAngle: 10,
              force: 0.5
            });

            const touchEvent = new TouchEvent(eventType, {
              cancelable: true,
              bubbles: true,
              touches: [touchObj],
              targetTouches: [touchObj],
              changedTouches: [touchObj],
              shiftKey: true
            });

            element.dispatchEvent(touchEvent);
          }

        function tapElement(element)
        {
            sendTouchEvent(element, 'touchstart');
            sendTouchEvent(element, 'touchend');
        }";
            browser.EvaluateScriptAsync(script);

        }

        private void ShortcutsWork(object sender, EventArgs e)
        {
            while (true)
            {
                if ((GetAsyncKeyState(0x31) & 0x8000) > 0)
                {
                    Refresh();
                }

                if ((GetAsyncKeyState(0x32) & 0x8000) > 0)
                {
                    InstantBuy();
                }


                Thread.Sleep(10);
            }

        }




        private void Refresh()
        {
            browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('ut-navigation-button-control')[0]);");
            Thread.Sleep(200);
            int currentValue;
            string value;

            value = GetValueByClass(browser, "numericInput", 0);

            Random rnd = new Random();
            int max = int.Parse(GetValueByClass(browser, "numericInput filled", GetElementCountByClass(browser, "numericInput filled") - 1));
            int number = rnd.Next(150 / 50, max / 50) * 50;
            int step;

            if (int.TryParse(value, out currentValue))
            {
                step = (number - currentValue) / 50;
            }
            else
            {
                step = (number - 150) / 50 + 1;
            }


            if (step > 0)
            {
                for (int i = 0; i < step; i++)
                {
                    browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-standard increment-value')[0]);");
                    Thread.Sleep(50);
                }
            }
            else
            {
                for (int i = 0; i > step; i--)
                {
                    browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-standard decrement-value')[0]);");
                    Thread.Sleep(50);
                }
            }
            Thread.Sleep(200);
            browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-standard call-to-action')[0]);");
        }

        public void InstantBuy()
        {
            browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-standard buyButton currency-coins')[0]);");
            Thread.Sleep(100);

            for (int z = 0; z < GetElementCountByClass(browser, "btn-text"); z++)
            {
                if (GetTextByClass(browser, "btn-text", z).ToUpper() == "OK")
                {
                    browser.EvaluateScriptAsync("tapElement(document.getElementsByClassName('btn-text')[" + z.ToString() + "]);");
                }
            }

        }

        public int GetElementCountByClass(ChromiumWebBrowser myCwb, string className)
        {
            try
            {
                string script = string.Format("(function() {{return document.getElementsByClassName('{0}').length;}})();", className);
                JavascriptResponse jr = myCwb.EvaluateScriptAsync(script).Result;
                return int.Parse(jr.Result.ToString());
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public string GetTextByClass(ChromiumWebBrowser myCwb, string className, int index)
        {
            try
            {
                string script = string.Format("(function() {{return document.getElementsByClassName('{0}')[{1}].innerText;}})();",
    className, index);
                JavascriptResponse jr = myCwb.EvaluateScriptAsync(script).Result;
                return jr.Result.ToString();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public string GetTextByTag(ChromiumWebBrowser myCwb, string tag, int index)
        {
            string script = string.Format("(function() {{return document.getElementsByTagName('{0}')[{1}].innerText;}})();",
                tag, index);
            JavascriptResponse jr = myCwb.EvaluateScriptAsync(script).Result;
            return jr.Result.ToString();
        }

        public string GetValueByClass(ChromiumWebBrowser myCwb, string className, int index)
        {
            try
            {
                string script = string.Format("(function() {{return document.getElementsByClassName('{0}')[{1}].value;}})();",
    className, index);
                JavascriptResponse jr = myCwb.EvaluateScriptAsync(script).Result;
                return jr.Result.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetElementValueById(ChromiumWebBrowser myCwb, string id)
        {
            string script = string.Format("(function() {{return document.getElementById('{0}').value;}})();",
                id);
            JavascriptResponse jr = myCwb.EvaluateScriptAsync(script).Result;
            return jr.Result.ToString();
        }

        public string GetElementValueByName(ChromiumWebBrowser myCwb, string name, int index)
        {
            string script = string.Format("(function() {{return document.getElementsByName('{0}')[{1}].value;}})();",
                name, index);
            JavascriptResponse jr = myCwb.EvaluateScriptAsync(script).Result;
            return jr.Result.ToString();
        }

        public void ChangePage(Page page)
        {
            if (currentPage != page)
            {
                currentPage = page;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Settings settingsForm = new Settings();
            settingsForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (botEnabled == true)
            {
                botEnabled = false;
                mainWorker.CancelAsync();
                mainWorker.Dispose();
                mainWorker = new BackgroundWorker();
                mainWorker.WorkerSupportsCancellation = true;

            }
            else
            {
                wait = false;
                botEnabled = true;
                mainWorker.DoWork += AutoBuy;
                mainWorker.RunWorkerAsync();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InstantBuy();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadingWorker.Dispose();
            loadingWorker.DoWork += WaitForLoaded;
            loadingWorker.RunWorkerAsync();
        }
    }
}
