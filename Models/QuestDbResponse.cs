namespace Authentication_Servie.Models
{
    public class QuestDbResponse<T>
    {
        public List<List<T>> dataset { get; set; } = new();
    }

}
