
namespace ComponentUI
{
    public class OSR : ComponentUIBase
    {
        public OSR()
            : base(new Core.Components.OSR())
        {
            Line2 = "[OSR]";
        }

        protected override void LogicComponent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }
    }
}
