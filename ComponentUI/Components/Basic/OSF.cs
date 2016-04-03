
namespace ComponentUI
{
    public class OSF : ComponentUIBase
    {
        public OSF()
            : base(new Core.Components.OSR())
        {
            Line2 = "[OSF]";
        }

        protected override void LogicComponent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}
