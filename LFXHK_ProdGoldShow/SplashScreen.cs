using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace LFXHK_ProdGoldShow
{
    [Activity(Theme = "@style/Theme.Splash", Icon = "@drawable/laofengxiang", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation|Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Thread.Sleep(2500);
            try
            {
                //Intent layOut = new Intent();
                //layOut.SetClass(this, typeof(MainActivity));
                //StartActivity(layOut);
                //this.Finish();
                Fun_GetGoldPrice();
            }
            catch 
            {
                this.Finish();
            } 
        } 
        private void Fun_GetGoldPrice()
        {
            string resultStringDr1 = null;
            Java.Lang.Thread th = new Java.Lang.Thread();
            th = new Java.Lang.Thread(() =>
            {
                try
                {
                    resultStringDr1 = WCFDataRequest.Instance.SvrRequest(
                    "P_GoldPriceInfo",
                        new string[] { },
                        new string[] { });
                }
                catch
                {
                    return;
                }
                if (resultStringDr1 != "连接错误" && resultStringDr1 != "连接超时")
                {
                    RunOnUiThread(() =>
                    {
                        Intent layOut = new Intent();
                        //layOut.SetClass(this, typeof(MainActivity));
                        layOut.SetClass(this, typeof(OtherMainActivity)); 
                        Bundle homeData = new Bundle();
                        homeData.PutString("resultStringDr1", resultStringDr1);
                        layOut.PutExtras(homeData); 
                        StartActivity(layOut);
                        this.Finish();

                    });
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        MessageBox.Show(this, resultStringDr1, "请检查网络或者联系服务商");
                    });
                }
            });
            th.Start();
        }
    }
}