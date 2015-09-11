package md5b0723c9e190b0cf9cc1973585708150d;


public class NetworkBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("EpiMessenger.NetworkBinder, EpiMessenger, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", NetworkBinder.class, __md_methods);
	}


	public NetworkBinder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == NetworkBinder.class)
			mono.android.TypeManager.Activate ("EpiMessenger.NetworkBinder, EpiMessenger, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public NetworkBinder (md5b0723c9e190b0cf9cc1973585708150d.NetworkService p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == NetworkBinder.class)
			mono.android.TypeManager.Activate ("EpiMessenger.NetworkBinder, EpiMessenger, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "EpiMessenger.NetworkService, EpiMessenger, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}

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
