using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;
using LFXHK_ProdGoldShow;

namespace LFXHK_ProdGoldShow
{
    class MyGallery : Gallery
    {
        public MyGallery(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }
        public MyGallery(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
        }
        public void setImageActivity(MainActivity context)
        {

        }
        public override bool OnFling(Android.Views.MotionEvent e1, Android.Views.MotionEvent e2, float velocityX, float velocityY)
        {
            if (isScrollingLeft(e1, e2))
            {
                OnKeyDown(Keycode.DpadLeft, null);
            }
            else
            {
                OnKeyDown(Keycode.DpadRight, null);
            }
            if (this.SelectedItemPosition == 0)
            {
                //// 实现后退功能
                this.SetSelection(MainActivity.picture.Length);
            }
            return false;
        }

        private bool isScrollingLeft(Android.Views.MotionEvent e1, Android.Views.MotionEvent e2)
        {
            return e2.GetX() > e1.GetX();
        }

        public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return base.OnScroll(e1, e2, distanceX, distanceY);
        }

        
    }
}
