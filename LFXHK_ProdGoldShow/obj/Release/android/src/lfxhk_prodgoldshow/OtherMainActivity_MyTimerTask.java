package lfxhk_prodgoldshow;


public class OtherMainActivity_MyTimerTask
	extends java.util.TimerTask
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_run:()V:GetRunHandler\n" +
			"";
		mono.android.Runtime.register ("LFXHK_ProdGoldShow.OtherMainActivity/MyTimerTask, LFXHK_ProdGoldShow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", OtherMainActivity_MyTimerTask.class, __md_methods);
	}


	public OtherMainActivity_MyTimerTask () throws java.lang.Throwable
	{
		super ();
		if (getClass () == OtherMainActivity_MyTimerTask.class)
			mono.android.TypeManager.Activate ("LFXHK_ProdGoldShow.OtherMainActivity/MyTimerTask, LFXHK_ProdGoldShow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void run ()
	{
		n_run ();
	}

	private native void n_run ();

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
