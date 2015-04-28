using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.Animations;
using Java.Lang;
using Java.IO;
using System.Data;
using System.Collections.Generic;
using Android.Support.V4.View;
using Java.Util;
using Android.Graphics;
using Android.Content.Res;
using Widget;
using Java.Lang.Reflect;
using Android.Util;
using System.Text.RegularExpressions;
using System.Globalization;

namespace LFXHK_ProdGoldShow
{
    [Activity(Label = "金價顯示", Icon = "@drawable/laofengxiang", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class OtherMainActivity : Activity
    {
        #region 全局变量
        /// <summary>
        /// 金价信息ListView
        /// </summary>
        public static ListView gplistview;
        /// <summary>
        /// 金价数据List
        /// </summary>
        public static List<GoldPriceInfo> gpListInfo;
        /// <summary>
        /// 视频View
        /// </summary>
        VideoView vv = null;
        /// <summary>
        /// 按钮打开视频文件
        /// </summary>
        Button btnOpenVedioFiles = null;
        /// <summary>
        /// 视频路径
        /// </summary>
        string vvpath = null;
        /// <summary>
        /// 常量-文件标示
        /// </summary>
        const int FILE_RESULT_CODE = 1;
        /// <summary>
        /// 循环图片资源
        /// </summary>
        public static int[] picture = { Resource.Drawable.Prod1, Resource.Drawable.Prod2, Resource.Drawable.Prod3, Resource.Drawable.Prod4, Resource.Drawable.Prod5, Resource.Drawable.Prod6, Resource.Drawable.Prod7, Resource.Drawable.Prod8, Resource.Drawable.Prod9 };
        /// <summary>
        /// 自定义MyGallery
        /// </summary>
        private static MyGallery pictureGallery = null;
        /// <summary>
        /// 常量-图片序号
        /// </summary>
        private static int index = 0;
        /// <summary>
        /// 相对布局-用以承载图片和视频View
        /// </summary>
        RelativeLayout rl = null;
        /// <summary>
        /// 计时器
        /// </summary>
        Timer timer = null;
        #endregion


        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.OtherMain);

            Initialize();

            Bundle homedata = this.Intent.Extras;
            string resultStringDr1 = homedata.GetString("resultStringDr1");
            loading(resultStringDr1);

            rl = FindViewById<RelativeLayout>(Resource.Id.RelativeLayout01);
            View layout = LayoutInflater.Inflate(Resource.Layout.ImageLayout, null);
            pictureGallery = layout.FindViewById<MyGallery>(Resource.Id.mygallery);
            rl.AddView(layout, new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent));
            ImageAdapter adapter = new ImageAdapter(this);
            pictureGallery.Adapter = adapter;
            timer = new Timer();
            timer.Schedule(task, 10000, 10000);


        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(Menu.None, 0, Menu.None, "刷新金價").SetIcon(Android.Resource.Drawable.IcMenuDirections);
            menu.Add(Menu.None, 1, Menu.None, "播放視頻").SetIcon(Android.Resource.Drawable.IcMenuDirections);
            menu.Add(Menu.None, 2, Menu.None, "退出系统").SetIcon(Android.Resource.Drawable.IcMenuDirections);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 0:
                    Fun_GetGoldPrice();
                    return true;
                case 1:
                    MessageBox.Show(this, "提示", "工程師正在努力研發中。。。");
                    //OpenVideo();
                    return true;
                case 3:
                    return true;
                case 2:
                    this.Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void OpenVideo()
        {
            if (timer != null)
            {
                timer.Cancel();
                timer = null;
            }
            rl.RemoveAllViews();
            View layout2 = LayoutInflater.Inflate(Resource.Layout.VideoLayout, null);
            vv = layout2.FindViewById<VideoView>(Resource.Id.videoView1);
            btnOpenVedioFiles = layout2.FindViewById<Button>(Resource.Id.OpenVedioFiles);
            btnOpenVedioFiles.Click += new EventHandler(btnOpenVedioFiles_Click);
            rl.AddView(layout2, new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent));
        }

        void btnOpenVedioFiles_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("*/*");
            intent.AddCategory(Intent.CategoryOpenable);
            try
            {
                StartActivityForResult(Intent.CreateChooser(intent, "选择视频路径"), FILE_RESULT_CODE);
            }
            catch (ActivityNotFoundException ex100)
            {
                Toast.MakeText(this, ex100.Message.ToString(), ToastLength.Short).Show();
            }
            btnOpenVedioFiles.Visibility = ViewStates.Gone;
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (FILE_RESULT_CODE == requestCode)
            {
                Android.Net.Uri uri = data.Data;
                vvpath = FileUtils.getPath(this, uri);
                vv.SetVideoPath(vvpath);
                MediaController media = new MediaController(this);
                vv.SetMediaController(media);
                vv.Start();
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
        public class FileUtils
        {
            public static string getPath(Context context, Android.Net.Uri uri)
            {
                if ("content".Equals(uri.Scheme))
                {
                    string[] projection = { "_data" };
                    Android.Database.ICursor cursor = null;
                    try
                    {
                        cursor = context.ContentResolver.Query(uri, projection, null, null, null);
                        int column_index = cursor.GetColumnIndexOrThrow("_data");
                        if (cursor.MoveToFirst())
                        {
                            return cursor.GetString(column_index);
                        }
                    }
                    catch (System.Exception e)
                    {
                        return e.Message;
                    }
                }
                else if ("file".Equals(uri.Scheme))
                {
                    return uri.Path;
                }
                return null;
            }
        }

        MyTimerTask task = new MyTimerTask();
        class MyTimerTask : TimerTask
        {
            public override void Run()
            {
                Message message = new Message();
                message.What = 2;
                index = pictureGallery.SelectedItemPosition;
                if (index % picture.Length == 0)
                {
                    index = 1;
                }
                else
                {
                    index++;
                }
                Console.WriteLine(string.Format("index ={0} ", index));
                handler.SendMessage(message);
            }
        }
        static MyHandler handler = new MyHandler();
        class MyHandler : Handler
        {
            public override void HandleMessage(Message msg)
            {
                base.HandleMessage(msg);
                switch (msg.What)
                {
                    case 2:
                        pictureGallery.SetSelection(index);
                        break;
                    default:
                        break;
                }
            }
        }
        protected override void OnDestroy()
        {
            if (timer != null)
            {
                timer.Cancel();
                timer = null;
            }
            base.OnDestroy();
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void Initialize()
        {
            if (ManageDevice.isConnectingToInternet(this) == false)
            {
                MessageBox.Show(this, "無可用網絡", "請檢查網絡");
                return;
            }
            HttpDownloadFile.CheckPackVerson(this);
            TextView txtDateString = FindViewById<TextView>(Resource.Id.txtDateString);
            txtDateString.Text = DataString.StringData();
        }
        private void loading(string resultStringDr1)
        {
            DataSet ds = WCFDataRequest.ConvertJSON2DataSet(resultStringDr1);
            TextView textView14 = this.FindViewById<TextView>(Resource.Id.textView14);
            TextView textView15 = this.FindViewById<TextView>(Resource.Id.textView15);
            textView14.Text = FN(double.Parse(ds.Tables[0].Rows[0]["SalePrice"].ToString()).ToString());
            textView15.Text = FN(double.Parse(ds.Tables[0].Rows[0]["BuyPrice"].ToString()).ToString());


            TextView textView17 = this.FindViewById<TextView>(Resource.Id.textView17);
            TextView textView18 = this.FindViewById<TextView>(Resource.Id.textView18);
            textView17.Text = ds.Tables[0].Rows[1]["SalePrice"].ToString();
            textView18.Text = ds.Tables[0].Rows[1]["BuyPrice"].ToString();

            TextView textView23 = this.FindViewById<TextView>(Resource.Id.textView23);
            TextView textView24 = this.FindViewById<TextView>(Resource.Id.textView24);
            textView23.Text = FN(double.Parse(ds.Tables[0].Rows[2]["SalePrice"].ToString()).ToString());
            textView24.Text = FN(double.Parse(ds.Tables[0].Rows[2]["BuyPrice"].ToString()).ToString());

            TextView textView29 = this.FindViewById<TextView>(Resource.Id.textView29);
            TextView textView30 = this.FindViewById<TextView>(Resource.Id.textView30);
            textView29.Text = ds.Tables[0].Rows[3]["SalePrice"].ToString();
            textView30.Text = ds.Tables[0].Rows[3]["BuyPrice"].ToString();

            TextView textView35 = this.FindViewById<TextView>(Resource.Id.textView35);
            TextView textView36 = this.FindViewById<TextView>(Resource.Id.textView36);
            textView35.Text = FN(double.Parse(ds.Tables[0].Rows[4]["SalePrice"].ToString()).ToString());
            textView36.Text = FN(double.Parse(ds.Tables[0].Rows[4]["BuyPrice"].ToString()).ToString());

            TextView textView39 = this.FindViewById<TextView>(Resource.Id.textView39);
            string strxml = this.GetString(Resource.String.str8);
            string HKD = strxml+ ds.Tables[1].Rows[0]["HKD"].ToString();
            textView39.Text = HKD;
        }
        private string FN(string num)
        {
            string newstr = string.Empty;
            Regex r = new Regex(@"(\d+?)(\d{3})*(\.\d+|$)");
            Match m = r.Match(num);
            newstr += m.Groups[1].Value;
            for (int i = 0; i < m.Groups[2].Captures.Count; i++)
            {
                newstr += "," + m.Groups[2].Captures[i].Value;
            }
            newstr += m.Groups[3].Value;
            return newstr;
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
                if (resultStringDr1 != "鏈接錯誤" && resultStringDr1 != "鏈接超時")
                {
                    RunOnUiThread(() =>
                    {
                        loading(resultStringDr1);
                    });
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        MessageBox.Show(this, resultStringDr1, "請檢查網絡或者聯系服務商");
                    });
                }
            });
            th.Start();
        }
        protected override void OnPause()
        {
            base.OnPause();
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
        protected override void OnStop()
        {
            base.OnStop();
        }
        /// <summary>
        /// 图片适配器
        /// </summary>
        class ImageAdapter : BaseAdapter
        {
            Activity context;
            HashMap dataCache = new HashMap ();
            public ImageAdapter(Activity context)
            {
                this.context = context;
            }

            public override int Count
            {
                get { return int.MaxValue; }
            }

            public override Java.Lang.Object GetItem(int position)
            {
                return picture[position % picture.Length];
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
            {
                ImageView imageView = new ImageView(context);
                Bitmap current = (Bitmap)dataCache.Get(position);
                if (current!=null)
                {
                    imageView.SetImageBitmap(current);
                }
                else
                {
                    current = readBitMap(context, picture[position % picture.Length]);
                    imageView.SetImageBitmap(current);
                    dataCache.Put(position, current);
                }

                //Bitmap btm = readBitMap(context, picture[position % picture.Length]);
                //imageView.SetImageBitmap(btm);
                imageView.SetScaleType(ImageView.ScaleType.FitXy);
                imageView.LayoutParameters = new Gallery.LayoutParams(
                        Gallery.LayoutParams.FillParent,
                        Gallery.LayoutParams.FillParent); 
                return imageView;
                
            }

            public static Bitmap readBitMap(Context context, int resId)
            {
                BitmapFactory.Options opt = new BitmapFactory.Options();
                opt.InPreferredConfig = Bitmap.Config.Rgb565;
                opt.InPurgeable = true;
                opt.InInputShareable = true;
                //获取资源图片   
                System.IO.Stream xx = context.Resources.OpenRawResource(resId);
                return BitmapFactory.DecodeStream(xx, null, opt);
            }
        }
    }
}

