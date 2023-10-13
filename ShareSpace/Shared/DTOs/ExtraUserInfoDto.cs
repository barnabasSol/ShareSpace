using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSpace.Shared.DTOs
{
    public class ExtraUserInfoDto
    {
        public string? ProfilePicUrl { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}
