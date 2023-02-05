namespace TableMode
{
    public class AspectFactory : IAspectFactory
    {
        private readonly IContentProvider _contentProvider;

        public AspectFactory(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public IAspect Create(string aspectId, int count)
        {
            var aspectModel = _contentProvider.GetAspectById(aspectId);

            return new Aspect(
                aspectModel.Id,
                aspectModel.Name,
                aspectModel.Order)
            {
                Count = count
            };
        }
    }
}