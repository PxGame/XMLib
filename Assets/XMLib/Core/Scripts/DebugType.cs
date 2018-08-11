namespace XM
{
    /// <summary>
    /// Debug 类型
    /// </summary>
    public enum DebugType
    {
        Debug = 1 << 0,
        Normal = 1 << 1,
        Warning = 1 << 2,
        Exception = 1 << 3,
        Error = 1 << 4,
        GG = 1 << 5,
    }
}