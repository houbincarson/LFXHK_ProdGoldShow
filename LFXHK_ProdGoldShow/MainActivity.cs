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

namespace LFXHK_ProdGoldShow
{
    [Activity(Label = "��۽��r", Icon = "@drawable/laofengxiang", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        #region ȫ�ֱ���
        /// <summary>
        /// �����ϢListView
        /// </summary>
        public static ListView gplistview;
        /// <summary>
        /// �������List
        /// </summary>
        public static List<GoldPriceInfo> gpListInfo;
        /// <summary>
        /// ��ƵView
        /// </summary>
        VideoView vv = null;
        /// <summary>
        /// ��ť����Ƶ�ļ�
        /// </summary>
        Button btnOpenVedioFiles = null;
        /// <summary>
        /// ��Ƶ·��
        /// </summary>
        string vvpath = null;
        /// <summary>
        /// ����-�ļ���ʾ
        /// </summary>
        const int FILE_RESULT_CODE = 1;
        /// <summary>
        /// ѭ��ͼƬ��Դ
        /// </summary>
        public static int[] picture = { 
                                          //Resource.Drawable.gallery1, Resource.Drawable.gallery2, Resource.Drawable.gallery3, Resource.Drawable.gallery4, Resource.Drawable.gallery5, Resource.Drawable.gallery6
                                      };
        /// <summary>
        /// �Զ���MyGallery
        /// </summary>
        private static MyGallery pictureGallery = null;
        /// <summary>
        /// ����-ͼƬ���
        /// </summary>
        private static int index = 0;
        /// <summary>
        /// ��Բ���-���Գ���ͼƬ����ƵView
        /// </summary>
        RelativeLayout rl = null;
        /// <summary>
        /// ��ʱ��
        /// </summary>
        Timer timer = null;
        #endregion


        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

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
            menu.Add(Menu.None, 0, Menu.None, "ˢ�½��").SetIcon(Android.Resource.Drawable.IcMenuDirections);
            menu.Add(Menu.None, 1, Menu.None, "������Ƶ").SetIcon(Android.Resource.Drawable.IcMenuDirections);
            menu.Add(Menu.None, 2, Menu.None, "�˳�ϵͳ").SetIcon(Android.Resource.Drawable.IcMenuDirections);
            menu.Add(Menu.None, 3, Menu.None, "��ͣ����").SetIcon(Android.Resource.Drawable.IcMenuDirections);
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
                    OpenVideo();
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
                StartActivityForResult(Intent.CreateChooser(intent, "ѡ����Ƶ·��"), FILE_RESULT_CODE);
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
        /// ��ʼ���ؼ�
        /// </summary>
        private void Initialize()
        {
            if (ManageDevice.isConnectingToInternet(this) == false)
            {
                MessageBox.Show(this, "�o���þW�j", "Ո�z��W�j");
                return;
            }
            HttpDownloadFile.CheckPackVerson(this);
            TextView txtDateString = FindViewById<TextView>(Resource.Id.txtDateString);
            txtDateString.Text = DataString.StringData();
        }
        private void loading(string resultStringDr1)
        {
            DataSet ds = WCFDataRequest.ConvertJSON2DataSet(resultStringDr1);
            gpListInfo = new List<GoldPriceInfo>(); 
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                gpListInfo.Add(new GoldPriceInfo()
                {
                    Id = ds.Tables[0].Rows[i]["Id"].ToString(),
                    Fineness = ds.Tables[0].Rows[i]["Fineness"].ToString(),
                    SalePrice = ds.Tables[0].Rows[i]["SalePrice"].ToString(),
                    BuyPrice = ds.Tables[0].Rows[i]["BuyPrice"].ToString()
                });
            }
            gplistview = FindViewById<ListView>(Resource.Id.gplistview);
            GoldPriceInfoAdapter adapter = new GoldPriceInfoAdapter(this, gpListInfo);
            gplistview.Adapter = adapter;

            string HKD = ds.Tables[1].Rows[0]["HKD"].ToString();
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
                if (resultStringDr1 != "朽��e�`" && resultStringDr1 != "朽ӳ��r")
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
                        MessageBox.Show(this, resultStringDr1, "Ո�z��W�j����ϵ������");
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
        /// ͼƬ������
        /// </summary>
        class ImageAdapter : BaseAdapter
        {
            Activity context;
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
                Bitmap btm = readBitMap(context, picture[position % picture.Length]);
                imageView.SetImageBitmap(btm);
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
                //��ȡ��ԴͼƬ   
                System.IO.Stream xx = context.Resources.OpenRawResource(resId);
                return BitmapFactory.DecodeStream(xx, null, opt);
            }
        }
    }
}

