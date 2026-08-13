namespace Memo.Bill.Application.Common.Interfaces.Services.Text;

public interface ISegmenterService
{ 
    /// <summary>
  /// 全模式\精确模式
  /// </summary>
  /// <param name="text"></param>
  /// <param name="cutAll">指定是否采用全模式</param>
  /// <param name="hmm">指定使用是否使用hmm模型切分未登录词</param>
  /// <returns></returns>
    public List<string> Cut(string text, bool cutAll = false, bool hmm = true);

    /// <summary>
    /// 全模式\精确模式
    /// </summary>
    /// <param name="text">原文</param>
    /// <param name="split">分隔符, 默认;号</param>
    /// <param name="cutAll">指定是否采用全模式</param>
    /// <param name="hmm">指定使用是否使用hmm模型切分未登录词</param>
    /// <returns></returns>
    public string CutWithSplit(string text, string split = ";", bool cutAll = false, bool hmm = true);

    /// <summary>
    /// 搜索引擎模式
    /// </summary>
    /// <param name="text">原文</param>
    /// <param name="hmm">指定使用是否使用hmm模型切分未登录词</param>
    /// <returns></returns>
    public List<string> CutForSearch(string text, bool hmm = true);

    /// <summary>
    /// 搜索引擎模式
    /// </summary>
    /// <param name="text">原文</param>
    /// <param name="split">分隔符, 默认;号</param>
    /// <param name="hmm">指定使用是否使用hmm模型切分未登录词</param>
    /// <returns></returns>
    public string CutWithSplitForSearch(string text, string split = ";", bool hmm = true);
}
