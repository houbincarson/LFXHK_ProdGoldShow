using System;  
using Android.App;  
using Android.Content;
namespace LFXHK_ProdGoldShow  
{  
    public class MessageBox  
    {  
		private static AlertDialog.Builder CreateDialog(Context ctx, string title, string message)  
		{  
			AlertDialog.Builder dlg = new AlertDialog.Builder(ctx);  

			return dlg.SetTitle(title).SetMessage(message);  
		}  
        public static void Show(Context ctx,string title, string message)  
        {  
            AlertDialog.Builder dlg = new AlertDialog.Builder(ctx);  
            dlg.SetTitle(title);  
            dlg.SetMessage(message);  
            dlg.SetPositiveButton("確定", delegate { });  
            dlg.Show(); 

        }  
        public static void ShowErrorMessage(Context ctx, Exception ex)  
        {  
            Show(ctx, "錯誤", ex.Message);  
        }  
		public static void Alert(Context ctx, string message)  
        {  
         CreateDialog(ctx, "提示", message).SetIcon(Android.Resource.Drawable.IcDialogAlert).SetPositiveButton("確定", delegate { }).Show();  
        }
		public static void Confirm(Context ctx, string title, string message, EventHandler<DialogClickEventArgs> okHandler, EventHandler<DialogClickEventArgs>  cancelHandler)  
		{  
			CreateDialog(ctx, title, message).SetPositiveButton("確定", okHandler).SetNegativeButton("取消", cancelHandler).Show(); 
		}  
		public static void Confirm(Context ctx, string title, string message,string OKName,string CancelName, EventHandler<DialogClickEventArgs> okHandler, EventHandler<DialogClickEventArgs>  cancelHandler)  
		{
			 CreateDialog(ctx, title, message).SetPositiveButton(OKName, okHandler).SetNegativeButton(CancelName, cancelHandler).Show();
		}
    }  
}  