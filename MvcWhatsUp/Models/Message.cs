namespace MvcWhatsUP.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }

        public Message(int id, int senderId, int receiverId, string text, DateTime sentAt)
        {
            MessageId = id;
            SenderUserId = senderId;
            ReceiverUserId = receiverId;
            MessageText = text;
            SentAt = sentAt;
        }
    }
}
