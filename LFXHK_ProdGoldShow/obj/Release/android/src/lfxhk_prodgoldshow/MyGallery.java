package lfxhk_prodgoldshow;


public class MyGallery
	extends android.widget.Gallery
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onFling:(Landroid/view/MotionEvent;Landroid/view/MotionEvent;FF)Z:GetOnFling_Landroid_view_MotionEvent_Landroid_view_MotionEvent_FFHandler\n" +
			"n_onScroll:(Landroid/view/MotionEvent;Landroid/view/MotionEvent;FF)Z:GetOnScroll_Landroid_view_MotionEvent_Landroid_view_MotionEvent_FFHandler\n" +
			"";
		mono.android.Runtime.register ("LFXHK_ProdGoldShow.MyGallery, LFXHK_ProdGoldShow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MyGallery.class, __md_methods);
	}


	public MyGallery (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == MyGallery.class)
			mono.android.TypeManager.Activate ("LFXHK_ProdGoldShow.MyGallery, LFXHK_ProdGoldShow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public MyGallery (android.content.Context p0, android.util.AttributeSet p1, int p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == MyGallery.class)
			mono.android.TypeManager.Activate ("LFXHK_ProdGoldShow.MyGallery, LFXHK_ProdGoldShow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public boolean onFling (android.view.MotionEvent p0, android.view.MotionEvent p1, float p2, float p3)
	{
		return n_onFling (p0, p1, p2, p3);
	}

	private native boolean n_onFling (android.view.MotionEvent p0, android.view.MotionEvent p1, float p2, float p3);


	public boolean onScroll (android.view.MotionEvent p0, android.view.MotionEvent p1, float p2, float p3)
	{
		return n_onScroll (p0, p1, p2, p3);
	}

	private native boolean n_onScroll (android.view.MotionEvent p0, android.view.MotionEvent p1, float p2, float p3);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
