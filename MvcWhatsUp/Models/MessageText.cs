namespace MvcWhatsUP.Models
{
    public class MessageText
    {
        public string Name { get; set; }
        public string MessageT { get; set; }

        public MessageText()
        {
            Name = "";
            MessageT = "";


        }
        public MessageText(string name, string messageText)
        {
            Name = name;
            MessageT = messageText;
        }



    }
}
