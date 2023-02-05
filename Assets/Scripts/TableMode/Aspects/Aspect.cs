namespace TableMode
{
    public class Aspect : IAspect
    {
        public bool IsActive { get; set; }
        public string Id { get; }
        public string Name { get; }
        public int Count { get; set; }

        public Aspect(
            string id,
            string name,
            int count)
        {
            Id = id;
            Name = name;
            Count = count;
            IsActive = true;
        }

        public void Update()
        {
            IsActive = true;
            if (Count > 1) Count--;
        }
    }
}