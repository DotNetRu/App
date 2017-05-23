# Getting Started with ToastIOS
---


##FirstStep  
    using ToastIOS;

___

##If you just want simplest pattern,use  :  
    Toast.MakeText("your show text").Show();

---

##If you want to show with icon:
###(Info icon) 
    Toast.MakeText("your show text").Show(ToastType.Info);
###(Notice icon)
    Toast.MakeText("your show text").Show(ToastType.Notice);
###(Warning icon)
    Toast.MakeText("your show text").Show(ToastType.Warning);
###(Error icon)
    Toast.MakeText("your show text").Show(ToastType.Error);

----
##If you accustom to android, You may use same pattern as android
    Toast.MakeText("Android Like Toast ?",Toast.LENGTH_SHORT).Show();
####or
    Toast.MakeText("Android Like Toast ?",Toast.LENGTH_LONG).Show();

----

##If you want to customize. You can add '.SetXXX(xxx)' after .MakeText(..) stackingly.
 such as 

###Set Where toast will be shown.(Top,Center,Bottom)
    Toast.MakeText("your show text")
        .SetGravity(ToastGravity.Top)
        .Show();

###Set How long toast will be shown in millisecond 
    Toast.MakeText("your show text")
        .SetDuration(3000)
        .Show();

###Set Where icon will be shown relative to text.(Top,Left)
    Toast.MakeText("your show text")
        .SetImageLocation(ToastImageLocation.Top)
        .Show(ToastType.Notice);

###Set Font Size :
    Toast.MakeText("your show text")
        .SetFontSize (15)
        .Show();

###Set Whether Toast have shadow or not.
    Toast.MakeText("your show text")
        .SetUseShadow(true)
        .Show();

###Set Corner Radius Size :
    Toast.MakeText("your show text")
        .SetCornerRadius (15)
        .Show();

###Set Everything Mix and Match
    Toast.MakeText("your show text")
    	.SetUseShadow(true)
    	.SetGravity(ToastGravity.Top)
        .SetCornerRadius (15)
        .SetDuration(3000)
        .SetImageLocation(ToastImageLocation.Left)
        .SetGravity(ToastGravity.Top)
        .Show(ToastType.Warning);

----