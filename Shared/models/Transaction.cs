using System;
using Newtonsoft.Json;
using ProtoBuf;

namespace Shared.models
{
    [ProtoContract]
    public class Transaction
    {
        [ProtoMember(1)]
        public string? FromAddress { get; set; }
        [ProtoMember(2)]
        public string ToAddress { get; set; }
        [ProtoMember(3)]
        public double Amount { get; set; }
        [ProtoMember(4)]
        [JsonIgnore]
        public string? Signature { get; set; }
    }
}

