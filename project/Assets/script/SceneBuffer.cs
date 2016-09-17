public class SceneBuffer{
    public string sceneName;

    private static SceneBuffer buffer;
    public static SceneBuffer GetBuffer()
    {
        if (buffer == null)
            buffer = new SceneBuffer();
        return buffer;
    }
}
