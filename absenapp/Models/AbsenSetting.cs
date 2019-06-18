using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace absenapp.Models
{
    [TableName("setting")]
    public class AbsenSetting
    {
        [PrimaryKey("idsetting")]
        [DbColumn("idsetting")]
        public int idsetting  { get; set; }

        [DbColumn("nominal")]
        public double nominal{ get; set; }

    }
}
