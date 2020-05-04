using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaStreamsFaulty.Origin.Messages
{
    public sealed class PrepareUpload
    {
        public int Id { get; }
        public PrepareUpload(int id)
        {
            Id = id;
        }
    }
}
