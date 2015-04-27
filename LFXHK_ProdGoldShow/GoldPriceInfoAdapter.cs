using Android.App;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LFXHK_ProdGoldShow
{
    /// <summary>
    /// 数据类
    /// </summary>
    public class GoldPriceInfo
    {
        public string Id
        {
            get;
            set;
        }
        public string Fineness
        {
            get;
            set;
        }
        public string SalePrice
        {
            get;
            set;
        }
        public string BuyPrice
        {
            get;
            set;
        }
    }
    
    public class GoldPriceInfoAdapter : BaseAdapter<GoldPriceInfo>
    {
        Activity context;
        public List<GoldPriceInfo> gpInfolist; 
        public GoldPriceInfoAdapter(Activity context, List<GoldPriceInfo> GoldPriceInfolist)
			: base()
		{
			this.context = context;
            this.gpInfolist = GoldPriceInfolist;
		}

        public override GoldPriceInfo this[int position]
        {
            get { return this.gpInfolist[position]; }
        }

        public override int Count
        {
            get { return this.gpInfolist.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var item = this.gpInfolist[position];
            var view = convertView; 
            if (convertView == null || !(convertView is LinearLayout))
                view = context.LayoutInflater.Inflate(Resource.Layout.goldpriceitem, parent, false); 
            
            var Fineness = view.FindViewById(Resource.Id.textView1) as TextView;
            var SalePrice = view.FindViewById(Resource.Id.textView2) as TextView;
            var BuyPrice = view.FindViewById(Resource.Id.textView3) as TextView; 

            Fineness.SetText(item.Fineness.ToString(), TextView.BufferType.Normal);
            SalePrice.SetText(FN(item.SalePrice.ToString()), TextView.BufferType.Normal);
            BuyPrice.SetText(FN(item.BuyPrice.ToString()), TextView.BufferType.Normal);

            return view;
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
        //禁止列表点击
        public override bool IsEnabled(int position)
        {
            return false;
        }
        public override bool AreAllItemsEnabled()
        {
            return false;
        } 
    }

}
