namespace ShareSpace.Shared.DTOs
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }
        public Guid From { get; set; }
        public required string To { get; set; }
        public required string Text { get; set; }
        public bool Seen { get; set; }
        public DateTime SentDateTime { get; set; }
    }

    public class UserMessageDto
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? Message { get; set; }
        public bool Seen { get; set; }
        public DateTime SentDateTime { get; set; }
    }
}
