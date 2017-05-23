using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;

namespace XamarinEvolve.Clients.UI
{
	public partial class CodeOfConductPage : BasePage
	{
		public override AppPage PageType => AppPage.CodeOfConduct;

        public CodeOfConductPage()
        {
            InitializeComponent();

			this.BindingContext = new CodeOfConductViewModel();

        }
	}
}

