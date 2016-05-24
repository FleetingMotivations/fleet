using System;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Fleet.Lattice.Model
{
    [DataContract]
    public class LatticeFile
    {
        [DataMember]
        public String FileName { get; set; }

        [DataMember]
        public Byte[] FileContents { get; set; }
    }
}
