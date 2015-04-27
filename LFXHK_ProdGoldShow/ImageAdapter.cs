using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Widget;
using Android.App;
using Android.Views;

namespace LFXHK_ProdGoldShow
{
    class ImageAdapter:BaseAdapter
    {
        private List<string> listurl;
        private Activity mContext; 
        public ImageAdapter(List<string> listurl)
        { 
            this.listurl = listurl;
        }

        public ImageAdapter(Activity mContext, List<string> listurl)
        {
            this.mContext = mContext;
            this.listurl = listurl;
        }
        public override int Count
        {
            get { return listurl.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return listurl[position].ToString();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var item = this.listurl[position];
            var view = convertView;
            if (convertView == null || !(convertView is LinearLayout))
                view = mContext.LayoutInflater.Inflate(Resource.Layout.item2, parent, false); 
            var image = view.FindViewById(Resource.Id.imageView1) as HttpImageView;
            image.SetHttpImageUrl(item);
            return view;
        }
    }
}
