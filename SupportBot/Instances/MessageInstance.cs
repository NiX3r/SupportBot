using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBot.Instances
{
    class MessageInstance
    {

        public String Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public String Content { get; set; }

        public MessageInstance(String author, DateTime createdAt, String content)
        {
            Author = author;
            CreatedAt = createdAt;
            Content = content;
        }

    }
}
