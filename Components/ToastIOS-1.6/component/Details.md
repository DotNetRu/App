# ToastIOS details
---
##Description
Unobstrusive Alert View. When you just want to tell something to user, But don't want to nag user for pressing any button. Fortunately Toast is your answer.
But Toast is exclusive for android. Not any more,With ToastIOS you can toast to user softly,simply.

##Features
- Simplest Usage.
- Unobstrusive Notication.
- With or without Icon.
- Customizable with tons of options.
- Unified Library

##For Simplest Usage
	
    Toast.MakeText("your show text").Show();
##For Android Fan Usage
For one who accustomer to android toast. You can use same syntax here.

    Toast.MakeText("your show text",Toast.LENGTH_LONG).Show();
##For Customizing Usage

    Toast.MakeText("your show text")
        .SetUseShadow(true)
        .SetGravity(ToastGravity.Top)
        .SetCornerRadius (15)
        .SetDuration(3000)
        .SetImageLocation(ToastImageLocation.Left)
        .SetGravity(ToastGravity.Top)
        .Show(ToastType.Warning);

### icon made by

'toast icon' created by Jacob Halton from the Noun Project

'warning icon','info icon','notice icon','error icon ' created by www.flaticon.com