using System.Collections.Generic;

namespace Shared.DTOs
{
    public class ActionResultDto<T>
    {
        public T Result { get; set; } = default(T);
        public List<string> Errors { get; set; } = new List<string>();
    }
}
