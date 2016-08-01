using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Common.Model
{
    public class ResultBaseModel
    {
        public bool Success { get; set; }

        public string Msg { get; set; }

        public string Code { get; set; }

        public int Id { get; set; }
    }

    public class EnumListModel
    {
        public string Title { get; set; }

        public int Id { get; set; }

        public bool IsCheck { get; set; }
    }
}
