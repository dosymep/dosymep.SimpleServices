using System;
using System.ComponentModel;

namespace dosymep.SimpleServices
{
    public interface IOpenDialogServiceBase
    {
        bool Multiselect { get; set; }
        bool ShowDialog(string directoryName);
    }
}