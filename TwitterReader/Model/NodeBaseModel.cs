using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterReader
{
    public class NodeBaseModel : BaseModel
    {
        public NodeBaseModel(string nodeImagePath)
        {            
            NodeImagePath = nodeImagePath;
            _isExpanded = true;
        }
        
        public string NodeImagePath { get; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
    }
}
