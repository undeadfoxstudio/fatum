namespace TableMode
{
    public interface IAspectViewFactory
    {
        IAspectView CreateAspect(string id, int count = 0);
        IAspectView CreateAntiAspect(string id, int count = 0);
    }
}