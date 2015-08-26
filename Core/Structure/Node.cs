
namespace Core.Components
{
    /// <summary>
    /// Circuit node
    /// Used as a point of connection between two or more components,
    /// </summary>
    public class Node : System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Node’s root component.
        /// Used for simulation purposes in order to avoid logical level involuntary resets
        /// </summary>
        public ComponentBase Root 
        {
            get { return _Root; }
            set
            {
                _Root = value;
                if (PropertyChanged != null) { PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("Root")); }
            } 
        }

        /// <summary>
        /// Node’s current logical level 
        /// </summary>
        public bool LogicLevel 
        {
            get { return _LogicLevel; }
            set
            {
                _LogicLevel = value;
                if (PropertyChanged != null) { PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("LogicLevel")); }
            }
        }

        /// <summary>
        /// Default builder
        /// </summary>
        public Node()
        {

        }

        /// <summary>
        /// Builder
        /// </summary>
        /// <param name="root">Owner component</param>
        public Node(ComponentBase root)
        {
            Root = root;
        }

        ComponentBase _Root;
        bool _LogicLevel;
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
