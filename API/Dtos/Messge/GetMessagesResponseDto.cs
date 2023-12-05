namespace API.Dtos.Messge
{
    public class GetMessagesResponseDto
    {
        public List<MessageDto> Messages { get; set; }
        public bool CanLoadMore { get; set; }
    }
}
