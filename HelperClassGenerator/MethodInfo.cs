using System.Collections.Generic;

namespace HelperClassGenerator
{
    public class MethodInfo
    {
        private List<string> _argumentList = null;

        public MethodInfo() { }

        public string MethodName { get; set; }

        public string MethodImplementation { get; set; }

        public int ArgumentCount { get; set; } = 0;

        public List<string> ArgumentList { get { return _argumentList; } set { _argumentList = value; }  }
    }
}
