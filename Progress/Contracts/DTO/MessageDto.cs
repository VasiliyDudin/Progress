using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO
{


    public class MessageDto<T>
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public T Payload { get; set; }

        public MessageDto(Guid uid, T payload)
        {
            Uid = uid;
            Payload = payload;
        }

        public MessageDto<TAnswer> CreateAnswer<TAnswer>(TAnswer payload)
        {
            return new MessageDto<TAnswer>(Uid, payload);
        }
    }
}
