namespace PracticeProject.Helpers
{
    public class TodoItemQueryObject
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? SortBy { get; set; } = null;
        public bool IsDecesending { get; set; } = false;
        public int Offset { get; set; } = 1;
        public int Limit { get; set; } = 20;
    }
}