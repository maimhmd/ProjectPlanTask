using System.Collections.Generic;

namespace OneTrack.PM.Entities.DTOs.Shared
{
    public class NodeDTO
    {
        public NodeDTO()
        {
            Children = new List<NodeDTO>();
        }
        public int Value { get; set; }
        public int ParentId { get; set; }
        public string Code { get; set; }
        public string Text { get; set; }
        public bool Collapsed { get; set; }
        public byte Lvl { get; set; }
        public byte? DomainId { get; set; }
        public List<NodeDTO> Children { get; set; }
    }
}

