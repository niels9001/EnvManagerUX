using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using EnvManagerUX.Classes;

namespace EnvManagerUX.Classes
{
    public class EnvVar
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public bool IsWarning { get; set; } = false;
    }
}